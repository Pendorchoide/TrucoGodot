namespace TrucoProject.Net.Events
{
    public class NetEvent
    {
        public enum Type
        {
            Connected,
            ConnectionFailed,
            Disconnected,

            MessageReceived,
            PingReceived
        }

        public Type EventType { get; }
        public object Payload { get; }

        public NetEvent(Type type, object payload = null)
        {
            EventType = type;
            Payload = payload;
        }
    }
}
