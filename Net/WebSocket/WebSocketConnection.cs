using Godot;

namespace TrucoProject.Net.WebSocket

/*
Wrapper sobre WebSocketPeer de Godot.
Se encarga de conectar, enviar, recibir, cerrar, pero NO decide reintentos ni parsing.
*/

{
    public class WebSocketConnection
    {
        private WebSocketPeer _peer;

        public WebSocketConnection()
        {
            _peer = new WebSocketPeer();
        }

        public Error ConnectToUrl(string url)
        {
            return _peer.ConnectToUrl(url);
        }

        public WebSocketPeer GetPeer() => _peer;

        public void Poll()
        {
            _peer.Poll();
        }

        public bool IsConnected()
        {
            return _peer.GetReadyState() == WebSocketPeer.State.Open;
        }

        public string? ReceiveMessage()
        {
            while (_peer.GetAvailablePacketCount() > 0)
            {
                var raw = _peer.GetPacket().GetStringFromUtf8();
                return raw;
            }

            return null;
        }

        public void Send(string text)
        {
            if (IsConnected())
                _peer.SendText(text);
        }

        public void Close()
        {
            if (_peer.GetReadyState() != WebSocketPeer.State.Closed)
                _peer.Close();
        }
    }
}
