namespace TrucoProject.Net.Protocol
{
    /// <summary>
    /// Canonical protocol message & event keys.
    /// Do NOT change values unless the server protocol changes.
    /// </summary>
    public static class ProtocolKeys
    {
        // =====================
        // Core / Infra
        // =====================
        public const string Event      = "event";
        public const string Upgrade    = "upgrade";
        public const string Connection = "connection";
        public const string Message    = "message";
        public const string Close      = "close";
        public const string Error      = "error";

        // =====================
        // Heartbeat
        // =====================
        public const string Ping = "ping";
        public const string Pong = "pong";

        // =====================
        // Lobby lifecycle
        // =====================
        public const string CreateLobby      = "create_lobby";
        public const string CreateLobbyOk    = "create_lobby_ok";
        public const string CreateLobbyError = "create_lobby_err";

        public const string JoinLobby      = "join_lobby";
        public const string JoinLobbyOk    = "join_lobby_ok";
        public const string JoinLobbyError = "join_lobby_err";

        public const string LeaveLobby       = "leave_lobby";
        public const string PlayerJoinLobby  = "player_join_lobby";
        public const string PlayerLeaveLobby = "player_leave_lobby";

        public const string LobbyNewOwner = "lobby_new_owner";
        public const string LobbyReady    = "lobby_ready";
    }
}
