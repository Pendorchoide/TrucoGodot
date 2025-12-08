namespace TrucoProject.Net.Messages
{
    public class BroadcastMessage : MessageBase
    {
        public string Text { get; set; } = "";

        public BroadcastMessage() : base("broadcast") { }
    }
}
