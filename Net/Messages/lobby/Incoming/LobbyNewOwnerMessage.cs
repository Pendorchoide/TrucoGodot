namespace TrucoProject.Net.Messages
{
    public class LobbyNewOwnerMessage : MessageBase {
        public string Owner {get; set;}
        
        public LobbyNewOwnerMessage (
            string owner
        ) {
            Type = Protocol.ProtocolKeys.LobbyNewOwner;
            Owner = owner; 
        }
    }
}