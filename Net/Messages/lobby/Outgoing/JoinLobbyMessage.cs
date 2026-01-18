namespace TrucoProject.Net.Messages {
    public class JoinLobbyMessage : MessageBase {
        public string lobbyId { get; set; }

        public JoinLobbyMessage(string _lobbyId) {
            type = Protocol.ProtocolKeys.JoinLobby;
            lobbyId = _lobbyId;
        }
    }
}