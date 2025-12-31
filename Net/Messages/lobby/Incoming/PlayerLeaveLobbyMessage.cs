namespace TrucoProject.Net.Messages
{
    public class PlayerLeaveLoobyMessage : MessageBase {
        public string PlayerId {get; set;}
        
        public string LobbyId {get; set;}
        public string PlayerName {get; set;}
        public PlayerLeaveLoobyMessage (
            string playerId,
            string lobbyId
        ) {
            Type = Protocol.ProtocolKeys.PlayerLeaveLobby;
            PlayerId = playerId; 
            LobbyId = lobbyId;
        }
    }
}