namespace TrucoProject.Net.Messages
{
    public class LoobyReadyMessage : MessageBase {
        public string LobbyId {get; set;}
        
        public LoobyReadyMessage (
            string lobbyId
        ) {
            Type = Protocol.ProtocolKeys.LobbyReady;
            LobbyId = LobbyId; 
        }
    }
}