using System.Collections.Generic;
using TrucoProject.Net.Protocol;

namespace TrucoProject.Net.Messages
{
    public class CreateLobbyOkMessage : MessageBase {
        public string LobbyId { get; set; }
        public string Owner { get; set; }
        public int MaxPlayers { get; set; }

        public List<string> Players { get; set; } = new();

        public CreateLobbyOkMessage()
        {
            Type = ProtocolKeys.CreateLobbyOk;
        }
    }
}
