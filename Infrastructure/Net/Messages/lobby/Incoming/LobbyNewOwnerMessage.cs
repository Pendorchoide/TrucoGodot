namespace TrucoProject.Net.Messages
{
    public class LobbyNewOwnerMessage : MessageBase {
        public string NewOwner {get; set;}
        public string LobbyId {get; set;}

        public LobbyNewOwnerMessage (
            string newOwner,
            string lobbyId
        ) {
            type = Protocol.ProtocolKeys.LobbyNewOwner;
            NewOwner = newOwner;
            LobbyId = lobbyId;
        }
    }
}