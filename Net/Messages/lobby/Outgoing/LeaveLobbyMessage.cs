namespace TrucoProject.Net.Messages {
    public class LeaveLobbyMessage : MessageBase {
        public LeaveLobbyMessage() {
            Type = Protocol.ProtocolKeys.LeaveLobby;
        }
    }
}