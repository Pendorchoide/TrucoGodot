namespace TrucoProject.Net.Messages
{
    public class JoinRoomResultMessage : MessageBase
    {
        public string PlayerId { get; set; } = "";
        public string RoomId { get; set; } = "";

        public JoinRoomResultMessage() : base("joinRoom") {}
    }
}
