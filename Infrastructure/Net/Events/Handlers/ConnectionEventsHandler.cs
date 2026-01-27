using Godot;
using TrucoProject.Net.Messages;
using TrucoProject.Net.Utils;
using TrucoProject.Net.WebSocket;

namespace TrucoProject.Net.Events.Handlers
{
    public class ConnectionEventsHandler {
        public ConnectionEventsHandler() {
            NetEventBus.Subscribe(NetEvent.Type.Connected, OnConnected);
            NetEventBus.Subscribe(NetEvent.Type.ConnectionFailed, OnConnectionFailed);
            NetEventBus.Subscribe(NetEvent.Type.Disconnected, OnDisconnected);
            NetEventBus.Subscribe(NetEvent.Type.Ping, OnPing);
            NetEventBus.Subscribe(NetEvent.Type.Pong, OnPong);
        }

        private void OnConnected(NetEvent evt) {
            GD.Print("[NET] Conectado al servidor");
        }

        private void OnConnectionFailed(NetEvent evt) {
            GD.PrintErr("[NET] Falló la conexión al servidor");
        }

        private void OnDisconnected(NetEvent evt) {
            // TODO: Show the reconnection interface, reconnect, etc.
            GD.Print("[NET] Desconectado del servidor");
        }

        private void OnPing (NetEvent evt) {
            PongMessage msg = new PongMessage();
            WebSocketClient.GetInstance().Send(msg);
        }
        private void OnPong (NetEvent evt) {
            Heartbeat.GetInstance().OnPong();
        }
    }
}
