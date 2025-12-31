namespace TrucoProject.Net.Messages
{
    public class PlayerJoinLoobyMessage : MessageBase {
        public string PlayerId {get; set;}
        
        public string LobbyId {get; set;}
        public string PlayerName {get; set;}
        public PlayerJoinLoobyMessage (
            string playerId,
            string lobbyId,
            string playerName
        ) {
            Type = Protocol.ProtocolKeys.PlayerJoinLobby;
            PlayerId = playerId; 
            LobbyId = lobbyId;
            PlayerName = playerName;
        }
    }
}