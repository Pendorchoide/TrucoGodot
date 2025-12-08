namespace TrucoProject.Net.Messages
{
    public class PlayCardResultMessage : MessageBase
    {
        public string PlayerId { get; set; } = "";
        public string CardId { get; set; } = "";
        public bool Valid { get; set; }
        
        // opcional: datos del nuevo estado del juego
        public object? NewState { get; set; }

        public PlayCardResultMessage() : base("playCard") { }
    }
}
