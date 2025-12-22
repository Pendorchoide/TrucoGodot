using System;
using System.Text.Json;

namespace TrucoProject.Net.Messages
{
    // Parses raw JSON and returns an instance of MessageBase (or subclass)
    public static class MessageParser {
        public static MessageBase? Parse(string raw) {
            try {
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;

                if (!root.TryGetProperty("type", out var typeProp)) {
                    return null;
                }

                string type = typeProp.GetString() ?? "";

                // depending on the "type", we deserialize to the specific type
                return type switch {
                    // Inncoming
                    "ping" => JsonSerializer.Deserialize<ServerPingMessage>(root.GetRawText()),
                    "pong" => JsonSerializer.Deserialize<ServerPongMessage>(root.GetRawText()),
                    "createRoomRes" => JsonSerializer.Deserialize<CreateRoomResultMessage>(raw),

                    // Outgoing
                    "createRoom" => JsonSerializer.Deserialize<ServerPongMessage>(root.GetRawText()),
                    "joinRoom" => JsonSerializer.Deserialize<JoinRoomResultMessage>(raw),

                    _ => new GenericMessage(type, raw)
                };
            }
            catch (Exception e) {
                Godot.GD.PrintErr("[MessageParser] Error parseando JSON: ", e.Message);
                return null;
            }
        }
    }

    // Fallback for untyped messages
    public class GenericMessage : MessageBase {
        public string Raw { get; set; }

        public GenericMessage(string type, string raw): base(type) {
            Raw = raw;
        }
    }
}
