namespace TrucoProject.Net.Messages
{
    public class JoinLobbyOkMessage: MessageBase {
        public string LobbyId {get; set;}
        public string Owner {get; set;}
        public int MaxPlayers {get; set;}
        public string Players {get; set;}
        
        public JoinLobbyOkMessage(
            string lobbyId,
            string owner, 
            int maxPlayers, 
            string players
        ) {
            Type = Protocol.ProtocolKeys.JoinLobbyOk;
            LobbyId = lobbyId;
            Owner = owner; 
            MaxPlayers = maxPlayers; 
            Players = players;
        }
    }
}