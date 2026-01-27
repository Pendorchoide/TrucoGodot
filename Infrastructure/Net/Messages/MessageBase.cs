using System.Text.Json.Serialization;

namespace TrucoProject.Net.Messages
{
    public abstract class MessageBase {
        [JsonPropertyName("type")]
        public string type { get; set; }

        protected MessageBase() { }

        protected MessageBase(string _type) {
            type = _type;
        }
    }
}
