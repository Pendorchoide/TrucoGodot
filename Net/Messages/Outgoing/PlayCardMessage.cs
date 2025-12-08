namespace TrucoProject.Net.Messages
{
    public class PlayCardMessage
    {
        public string Type { get; set; } = "playCard";
        public string PlayerId { get; set; } = "";
        public string CardId { get; set; } = "";
        public string TargetId { get; set; } = ""; // opcional
    }
}
