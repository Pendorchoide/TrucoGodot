namespace TrucoProject.Net.Events
{
    public class NetEvent {
        public enum Type {
            // Infra
            Connected,
            ConnectionFailed,
            Disconnected,
            // Raw
            MessageReceived,
            ConnectionOk, // The server sends user data when a successful connection is established
            // Heartbeat
            Ping,
            Pong,

            // Lobby
            CreateLobby,
            CreateLobbyOk,
            CreateLobbyErr,
            JoinLobby,
            JoinLobbyOk,
            JoinLobbyErr,
            LeaveLobby,
            PlayerJoinLobby,
            PlayerLeaveLobby,
            LobbyNewOwner,
            LobbyReady, 

            // Game 
            GameStarted
        }

        public Type EventType { get; }
        public object Payload { get; }

        public NetEvent(Type type, object payload = null) {
            EventType = type;
            Payload = payload;
        }
    }
}
