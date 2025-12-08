namespace TrucoProject.Net.Messages

{
    public class JoinRoomMessage
    {
        public string Type { get; set; } = "joinRoom";
        public string RoomId { get; set; } = "";
        public string PlayerId { get; set; } = "";
    }
}
