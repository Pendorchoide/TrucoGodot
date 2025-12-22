using System.Text.Json.Serialization;

namespace TrucoProject.Net.Messages
{
    public class JoinRoomMessage : MessageBase
    {
        [JsonPropertyName("roomId")]
        public string RoomId { get; set; }

        public JoinRoomMessage() : base("joinRoom") { }
    }
}
