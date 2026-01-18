namespace TrucoProject.Net.Messages
{
    public class JoinLobbyErrMessage: MessageBase {
        public string Message {get; set;}
        
        public JoinLobbyErrMessage(string message) {
            type = Protocol.ProtocolKeys.JoinLobbyError;
            Message = message;
        }
    }
}