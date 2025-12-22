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
    public partial class WebSocketClient : Node {

        public WebSocketConfig Config { get; private set; }
        public WebSocketState State { get; private set; } = WebSocketState.Disconnected;

        private Heartbeat _heartbeat;

        private ClientWebSocket _socket;
        private CancellationTokenSource _cts;

        private readonly Queue<string> _sendQueue = new();
        private readonly byte[] _buffer = new byte[8192];

        public static WebSocketClient Instance { get; private set; }
        public override void _EnterTree() {
            Instance = this;
        }

        public WebSocketClient() {
            Instance = this;
        }

        public async Task ConnectAsync(WebSocketConfig config) {
            if (State != WebSocketState.Disconnected) return;

            Config = config;
            _cts = new CancellationTokenSource();
            _socket = new ClientWebSocket();
            State = WebSocketState.Connecting;

            NetLogger.Info($"[WS] Connecting to {config.Url}");

            try {
                await _socket.ConnectAsync(new Uri(config.Url), _cts.Token);
            } 
            catch (Exception e) {
                NetLogger.Error($"[WS] Connection error: {e.Message}");
                State = WebSocketState.Disconnected;

                NetEventBus.Emit(new NetEvent(NetEvent.Type.ConnectionFailed, e));
                return;
            }

            State = WebSocketState.Connected;
            NetLogger.Info("[WS] Connected OK");

            NetEventBus.Emit(new NetEvent(NetEvent.Type.Connected));

            _heartbeat = new Heartbeat(this, config.HeartbeatInterval);

            // Start receiving loop
            _ = Task.Run(ReceiveLoop);
        }

        public async Task DisconnectAsync() {
            if (State == WebSocketState.Disconnected) return;

            NetLogger.Warn("[WS] Disconnecting...");
            State = WebSocketState.Disconnecting;

            try {
                _cts.Cancel();
                
                await _socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Client close", 
                    CancellationToken.None
                );
            }
            catch (Exception e) {
                NetLogger.Error(e.Message);
            }

            State = WebSocketState.Disconnected;
            NetLogger.Warn("[WS] Disconnected");

            NetEventBus.Emit(new NetEvent(NetEvent.Type.Disconnected));
        }

        public void Send(MessageBase msg) {
            string json = NetJson.Stringify(msg);
            _sendQueue.Enqueue(json);

            FlushSendQueue();
        }

        private async void FlushSendQueue() {
            if (_socket == null || _socket.State != System.Net.WebSockets.WebSocketState.Open) return;

            while (_sendQueue.Count > 0) {
                string raw = _sendQueue.Dequeue();
                byte[] bytes = Encoding.UTF8.GetBytes(raw);

                try {
                    await _socket.SendAsync(bytes, WebSocketMessageType.Text, true, _cts.Token);
                }
                catch (Exception e) {
                    NetLogger.Error($"[WS] Failed to send: {e.Message}");
                }
            }
        }

        private async Task ReceiveLoop() {
            NetLogger.Info("[WS] Receive loop started");

            while (!_cts.IsCancellationRequested) {
                if (_socket.State != System.Net.WebSockets.WebSocketState.Open) {
                    NetLogger.Warn("[WS] Socket closed unexpectedly");
                    break;
                }

                WebSocketReceiveResult result;

                try {
                    result = await _socket.ReceiveAsync(_buffer, _cts.Token);
                }
                catch (Exception e) {
                    NetLogger.Error($"[WS] Receive error: {e.Message}");
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Close) {
                    NetLogger.Warn("[WS] Server closed connection");
                    await DisconnectAsync();
                    return;
                }

                string json = Encoding.UTF8.GetString(_buffer, 0, result.Count);
                
                NetEventBus.Emit(
                    new NetEvent(NetEvent.Type.MessageReceived, json)
                );
            }
        }

        /*
        private void HandleIncoming(string json) {
            MessageBase msg;

            try {
                msg = MessageParser.Parse(json);
            }
            catch (Exception e) {
                NetLogger.Error($"[WS] Invalid message: {e.Message}");
                return;
            }

            // Built-in handling
            switch (msg) {
                case ServerPingMessage:
                    Send(new ClientPongMessage());
                break;

                case ServerPongMessage:
                    _heartbeat.OnPong();                
                break;
            }

            // Forward to game
            if (msg is not ServerPingMessage && msg is not ServerPongMessage) {
                NetEventBus.Emit(new NetEvent(NetEvent.Type.MessageReceived, msg));
            }
        }*/
    }
}
