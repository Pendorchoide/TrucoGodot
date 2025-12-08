using System.Collections.Generic;

namespace TrucoProject.Net.Messages
{
    // Ejemplo de estado de juego: adapta los campos a tus necesidades
    public class GameStateMessage : MessageBase
    {
        public int Turn { get; set; }
        public string CurrentPlayerId { get; set; } = "";
        public List<string> Players { get; set; } = new();

        public GameStateMessage() : base("gameState") { }
    }
}
