namespace TrucoProject.Net.Messages {
    public class JoinLobbyMessage : MessageBase {
        public string LobbyId { get; set; }

        public JoinLobbyMessage(string lobbyId) {
            Type = Protocol.ProtocolKeys.JoinLobby;
            LobbyId = lobbyId;
        }
    }
}