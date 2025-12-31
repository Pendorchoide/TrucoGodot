
namespace TrucoProject.Net.Messages
{
    public class CreateLobbyErrMessage: MessageBase {
        public string Message {get; set;}
        
        public CreateLobbyErrMessage(string message) {
            Type = Protocol.ProtocolKeys.CreateLobbyError;
            Message = message;
        }
    }
}
