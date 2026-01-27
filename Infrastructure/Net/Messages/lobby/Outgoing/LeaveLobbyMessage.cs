namespace TrucoProject.Net.Messages {
    public class LeaveLobbyMessage : MessageBase {
        public LeaveLobbyMessage() {
            type = Protocol.ProtocolKeys.LeaveLobby;
        }
    }
}