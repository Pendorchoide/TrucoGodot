namespace TrucoProject.Net.Messages {
    public class CreateLobbyMessage : MessageBase {
        public int MaxPlayers {get; set; }  
        public CreateLobbyMessage(int maxPlayer) {
            Type = Protocol.ProtocolKeys.CreateLobby;
            MaxPlayers = maxPlayer;
        }
    }
}