using System;
using System.Text.Json;
using TrucoProject.Net.Protocol;

namespace TrucoProject.Net.Messages
{
    public static class MessageParser {
        public static MessageBase? Parse(string raw) {
            try {
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;

                if (!root.TryGetProperty("type", out var typeProp)) {
                    return null;
                }
                string type = typeProp.GetString() ?? "";

                return type switch {
                    ProtocolKeys.Ping => JsonSerializer.Deserialize<ServerPingMessage>(root.GetRawText()),
                    ProtocolKeys.Pong => JsonSerializer.Deserialize<ServerPongMessage>(root.GetRawText()),
                    
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
