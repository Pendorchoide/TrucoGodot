namespace TrucoProject.Net.Messages
{
    public class PlayerJoinedMessage : MessageBase
    {
        public string PlayerId { get; set; } = "";
        public string Name { get; set; } = "";

        public PlayerJoinedMessage() : base("playerJoined") { }
    }
}
