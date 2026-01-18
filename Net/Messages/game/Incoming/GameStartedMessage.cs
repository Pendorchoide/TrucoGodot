namespace TrucoProject.Net.Messages
{
    public class GameStartedMessage: MessageBase {
        public string GameId { get; set; }
        public GameStartedMessage() {
            type = Protocol.ProtocolKeys.GameStarted;
        }
    }
}