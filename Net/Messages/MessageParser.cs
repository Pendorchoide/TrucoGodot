using System;
using System.Text.Json;
using TrucoProject.Net.Protocol;

namespace TrucoProject.Net.Messages
{
    public static class MessageParser {
        
        public static MessageBase? Parse(string raw) {
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true 
            };

            try {
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;

                if (!root.TryGetProperty("type", out var typeProp)) {
                    return null;
                }
                string type = typeProp.GetString() ?? "";

                return type switch {
                    // ======= HEARTBEAT ======= 
                    ProtocolKeys.Ping => JsonSerializer.Deserialize<ServerPingMessage>(root.GetRawText(), options),
                    ProtocolKeys.Pong => JsonSerializer.Deserialize<ServerPongMessage>(root.GetRawText(), options),
                   
                    // ======= LOBBY ======= 
                    ProtocolKeys.CreateLobbyError => JsonSerializer.Deserialize<CreateLobbyErrMessage>(root.GetRawText(), options),
                    ProtocolKeys.CreateLobbyOk => JsonSerializer.Deserialize<CreateLobbyOkMessage>(root.GetRawText(), options),
                    ProtocolKeys.JoinLobbyError => JsonSerializer.Deserialize<JoinLobbyErrMessage>(root.GetRawText(), options),
                    ProtocolKeys.JoinLobbyOk => JsonSerializer.Deserialize<JoinLobbyOkMessage>(root.GetRawText(), options),
                    ProtocolKeys.PlayerJoinLobby => JsonSerializer.Deserialize<PlayerJoinLoobyMessage>(root.GetRawText(), options),
                    ProtocolKeys.PlayerLeaveLobby => JsonSerializer.Deserialize<PlayerLeaveLoobyMessage>(root.GetRawText(), options),
                    ProtocolKeys.LobbyNewOwner => JsonSerializer.Deserialize<LobbyNewOwnerMessage>(root.GetRawText(), options),

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