namespace TrucoProject.Net.Messages

{
    public class ThreadIdMessage : MessageBase
    {
        public string Text { get; set; } = "";

        public ThreadIdMessage() : base("threadId") { }
    }
}