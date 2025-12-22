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

            PlayerJoinLobby,
            PlayerLeaveLobby,
            LobbyNewOwner,
            LobbyReady
        }

        public Type EventType { get; }
        public object Payload { get; }

        public NetEvent(Type type, object payload = null) {
            EventType = type;
            Payload = payload;
        }
    }
}
