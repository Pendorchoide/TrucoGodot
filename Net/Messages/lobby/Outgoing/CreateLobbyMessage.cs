namespace TrucoProject.Net.Messages {
    public class CreateLobbyMessage : MessageBase {
        public int maxPlayers {get; set; }  
        public CreateLobbyMessage(int maxPlayer) {
            type = Protocol.ProtocolKeys.CreateLobby;
            maxPlayers = maxPlayer;
        }
    }
}