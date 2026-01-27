
namespace TrucoProject.Net.Messages
{
    public class CreateLobbyErrMessage: MessageBase {
        public string Message {get; set;}
        
        public CreateLobbyErrMessage(string message) {
            type = Protocol.ProtocolKeys.CreateLobbyError;
            Message = message;
        }
    }
}
