namespace TrucoProject.Net.Messages
{
    public class JoinLobbyErrMessage: MessageBase {
        public string Message {get; set;}
        
        public JoinLobbyErrMessage(string message) {
            Type = Protocol.ProtocolKeys.JoinLobbyError;
            Message = message;
        }
    }
}