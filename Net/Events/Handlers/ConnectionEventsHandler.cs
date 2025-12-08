using Godot;

namespace TrucoProject.Net.Events.Handlers
{
    public class ConnectionEventsHandler
    {
        public ConnectionEventsHandler()
        {
            NetEventBus.Subscribe(NetEvent.Type.Connected, OnConnected);
            NetEventBus.Subscribe(NetEvent.Type.ConnectionFailed, OnConnectionFailed);
            NetEventBus.Subscribe(NetEvent.Type.Disconnected, OnDisconnected);
        }

        private void OnConnected(NetEvent evt)
        {
            GD.Print("[NET] Conectado al servidor");
        }

        private void OnConnectionFailed(NetEvent evt)
        {
            GD.PrintErr("[NET] Falló la conexión al servidor");
        }

        private void OnDisconnected(NetEvent evt)
        {
            GD.Print("[NET] Desconectado del servidor");
            // TODO: mostrar UI de reconexión, reconectar, etc.
        }
    }
}
