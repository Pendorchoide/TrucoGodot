using System.Text.Json.Serialization;

namespace TrucoProject.Net.Messages
{
    public class RoomJoinedMessage : MessageBase
    {
        [JsonPropertyName("roomId")]
        public string RoomId { get; set; }

        [JsonPropertyName("players")]
        public string[] Players { get; set; }

        public RoomJoinedMessage() : base("roomJoined") { }
    }
}
