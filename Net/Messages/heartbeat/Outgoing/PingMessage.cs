
namespace TrucoProject.Net.Messages
{
    public class PingMessage : MessageBase {
        public PingMessage() {
            Type = Protocol.ProtocolKeys.Ping;
        }
    }
}
