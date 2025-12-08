namespace TrucoProject.Net.WebSocket
{
    public class WebSocketConfig
    {
        public string Url { get; set; }
        public int HeartbeatInterval { get; set; } = 5; // segundos
        public int ReconnectDelay { get; set; } = 3;    // segundos

        public WebSocketConfig(string url, int heartbeatInterval = 5, int reconnectDelay = 3)
        {
            Url = url;
            HeartbeatInterval = heartbeatInterval;
            ReconnectDelay = reconnectDelay;
        }
    }
}
