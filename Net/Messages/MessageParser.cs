using System;
using System.Text.Json;

namespace TrucoProject.Net.Messages
{
    public static class MessageParser
    {
        // Parsea JSON crudo y devuelve una instancia de MessageBase (o subclase)
        public static MessageBase? Parse(string raw)
        {
            try
            {
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;

                if (!root.TryGetProperty("type", out var typeProp))
                    return null;

                string type = typeProp.GetString() ?? "";

                // según el "type", deserializamos al tipo concreto
                return type switch
                {
                    "ping" => JsonSerializer.Deserialize<ServerPingMessage>(root.GetRawText()),
                    "pong" => JsonSerializer.Deserialize<PongMessage>(root.GetRawText()),
                    "threadId" => JsonSerializer.Deserialize<ThreadIdMessage>(root.GetRawText()),
                    "broadcast" => JsonSerializer.Deserialize<BroadcastMessage>(root.GetRawText()),
                    "gameState" => JsonSerializer.Deserialize<GameStateMessage>(root.GetRawText()),
                    "playerJoined" => JsonSerializer.Deserialize<PlayerJoinedMessage>(root.GetRawText()),

                    // outgoing types may be parsed too if the server echoes them
                    "joinRoom" => JsonSerializer.Deserialize<JoinRoomResultMessage>(raw),
					"playCard" => JsonSerializer.Deserialize<PlayCardResultMessage>(raw),

                    _ => new GenericMessage(type, raw)
                };
            }
            catch (Exception e)
            {
                Godot.GD.PrintErr("[MessageParser] Error parseando JSON: ", e.Message);
                return null;
            }
        }
    }

    // Fallback para mensajes no tipados
    public class GenericMessage : MessageBase
    {
        public string Raw { get; set; }

        public GenericMessage(string type, string raw) : base(type)
        {
            Raw = raw;
        }
    }
}
