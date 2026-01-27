using System.Collections.Generic;

namespace TrucoProject.Net.Messages
{
    public class JoinLobbyOkMessage: MessageBase {
        public string LobbyId { get; set; }
        public string Owner { get; set; }
        public byte MaxPlayers { get; set; }
        public List<string> Players { get; set; } = new();
        
        public JoinLobbyOkMessage() {
            type = Protocol.ProtocolKeys.JoinLobbyOk;
        }
    }
}