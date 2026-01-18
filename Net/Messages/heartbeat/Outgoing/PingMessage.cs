
namespace TrucoProject.Net.Messages
{
    public class PingMessage : MessageBase {
        public PingMessage() {
            type = Protocol.ProtocolKeys.Ping;
        }
    }
}
