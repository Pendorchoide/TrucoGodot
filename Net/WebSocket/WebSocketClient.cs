using Godot;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using TrucoProject.Net.Events;
using TrucoProject.Net.Messages;
using TrucoProject.Net.Utils;

namespace TrucoProject.Net.WebSocket
{
    public partial class WebSocketClient : Node
    {

        public WebSocketConfig Config { get; private set; }
        public WebSocketState State { get; private set; } = WebSocketState.Disconnected;

        private Heartbeat _heartbeat;

        private ClientWebSocket _socket;
        private CancellationTokenSource _cts;

        private readonly Queue<string> _sendQueue = new();
        private readonly byte[] _buffer = new byte[8192];


        public override void _Process(double delta)
        {
            FlushSendQueue();
        }

        // ------------------------------
        // CONNECT
        // ------------------------------
        public async Task ConnectAsync(WebSocketConfig config)
        {
            if (State != WebSocketState.Disconnected)
                return;

            Config = config;
            _cts = new CancellationTokenSource();
            _socket = new ClientWebSocket();

            State = WebSocketState.Connecting;
            NetLogger.Info($"[WS] Connecting to {config.Url}");

            try
            {
                await _socket.ConnectAsync(new Uri(config.Url), _cts.Token);
            }
            catch (Exception ex)
            {
                NetLogger.Error($"[WS] Connection error: {ex.Message}");
                State = WebSocketState.Disconnected;

                NetEventBus.Emit(new NetEvent(NetEvent.Type.ConnectionFailed, ex));
                return;
            }

            State = WebSocketState.Connected;
            NetLogger.Info("[WS] Connected OK");

            NetEventBus.Emit(new NetEvent(NetEvent.Type.Connected));

            // Start heartbeat
            _heartbeat = new Heartbeat(this, config.HeartbeatInterval);

            // Start receiving loop
            _ = Task.Run(ReceiveLoop);
        }

        // ------------------------------
        // DISCONNECT
        // ------------------------------
        public async Task DisconnectAsync()
        {
            if (State == WebSocketState.Disconnected)
                return;

            NetLogger.Warn("[WS] Disconnecting...");
            State = WebSocketState.Disconnecting;

            try
            {
                _cts.Cancel();
                await _socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Client close", 
                    CancellationToken.None
                );
            }
            catch { /* ignored */ }

            State = WebSocketState.Disconnected;
            NetLogger.Warn("[WS] Disconnected");

            NetEventBus.Emit(new NetEvent(NetEvent.Type.Disconnected));
        }

        // ------------------------------
        // SEND MESSAGES
        // ------------------------------
        public void Send(MessageBase msg)
        {
            string json = NetJson.Stringify(msg);
            _sendQueue.Enqueue(json);
        }


        private async void FlushSendQueue()
        {
            if (_socket == null || _socket.State != System.Net.WebSockets.WebSocketState.Open)
                return;

            while (_sendQueue.Count > 0)
            {
                string raw = _sendQueue.Dequeue();
                byte[] bytes = Encoding.UTF8.GetBytes(raw);

                try
                {
                    await _socket.SendAsync(bytes, WebSocketMessageType.Text, true, _cts.Token);
                }
                catch (Exception ex)
                {
                    NetLogger.Error($"[WS] Failed to send: {ex.Message}");
                }
            }
        }

        // ------------------------------
        // RECEIVE LOOP
        // ------------------------------
        private async Task ReceiveLoop()
        {
            NetLogger.Info("[WS] Receive loop started");

            while (!_cts.IsCancellationRequested)
            {
                if (_socket.State != System.Net.WebSockets.WebSocketState.Open)
                {
                    NetLogger.Warn("[WS] Socket closed unexpectedly");
                    break;
                }

                WebSocketReceiveResult result;

                try
                {
                    result = await _socket.ReceiveAsync(_buffer, _cts.Token);
                }
                catch (Exception ex)
                {
                    NetLogger.Error($"[WS] Receive error: {ex.Message}");
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    NetLogger.Warn("[WS] Server closed connection");
                    await DisconnectAsync();
                    return;
                }

                string json = Encoding.UTF8.GetString(_buffer, 0, result.Count);
                HandleIncoming(json);
            }
        }

        // ------------------------------
        // HANDLE INCOMING MESSAGE
        // ------------------------------
        private void HandleIncoming(string json)
        {
            MessageBase msg;

            try
            {
                msg = MessageParser.Parse(json);
            }
            catch (Exception ex)
            {
                NetLogger.Error($"[WS] Invalid message: {ex.Message}");
                return;
            }

            // Built-in handling
            switch (msg)
            {
                case ServerPingMessage ping:
                    HandlePing(ping);
                    return;
            }

            // Forward to game
            NetEventBus.Emit(new NetEvent(NetEvent.Type.MessageReceived, msg));
        }

        private void HandlePing(ServerPingMessage ping)
        {
            Send(new ClientPingMessage());
            NetEventBus.Emit(new NetEvent(NetEvent.Type.PingReceived));
        }
    }
}
