namespace TrucoProject.Net.Messages
{
    public class PlayerJoinLobbyMessage : MessageBase {
        public string PlayerId {get; set;}
        
        public string LobbyId {get; set;}
        public string PlayerName {get; set;}
        public PlayerJoinLobbyMessage (
            string playerId,
            string lobbyId,
            string playerName
        ) {
            type = Protocol.ProtocolKeys.PlayerJoinLobby;
            PlayerId = playerId; 
            LobbyId = lobbyId;
            PlayerName = playerName;
        }
    }
}