namespace TrucoProject.Net.Messages
{
    class ConnectionOkMessage : MessageBase {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public ConnectionOkMessage (string playerId, string playerName) {
            type = Protocol.ProtocolKeys.ConnectionOk;
            PlayerId = playerId;
            PlayerName = playerName;
        }
    }
}