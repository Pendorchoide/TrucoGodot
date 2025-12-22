using System.Text.Json.Serialization;

namespace TrucoProject.Net.Messages
{
    public abstract class MessageBase {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        protected MessageBase() { }

        protected MessageBase(string type) {
            Type = type;
        }
    }
}
