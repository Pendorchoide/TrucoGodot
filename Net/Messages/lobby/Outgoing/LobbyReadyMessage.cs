namespace TrucoProject.Net.Messages
{
    public class LobbyReadyMessage : MessageBase {
        public string lobbyId {get; set;}
        
        public LobbyReadyMessage (string _lobbyId) {
            type = Protocol.ProtocolKeys.LobbyReady;
            lobbyId = _lobbyId; 
        }
    }
}