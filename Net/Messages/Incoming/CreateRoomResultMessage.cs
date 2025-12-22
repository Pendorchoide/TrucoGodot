using System.Text.Json.Serialization;

namespace TrucoProject.Net.Messages
{
    public class CreateRoomResultMessage : MessageBase
    {
        [JsonPropertyName("room")]
        public string Rooom { get; set; }

        public CreateRoomResultMessage() : base("createRoomRes") {}
    }
}
