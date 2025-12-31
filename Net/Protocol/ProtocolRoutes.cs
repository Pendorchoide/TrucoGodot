using TrucoProject.Net.Events;
using TrucoProject.Net.Protocol;

public static class ProtocolRoutes
{
    public static void RegisterAll(MessageRouter router)
    {
        // Heartbeat
        router.RegisterProtocol(ProtocolKeys.Ping, NetEvent.Type.Ping);
        router.RegisterProtocol(ProtocolKeys.Pong, NetEvent.Type.Pong);

        // Lobby
        router.RegisterProtocol(ProtocolKeys.CreateLobbyOk, NetEvent.Type.CreateLobbyOk);
        router.RegisterProtocol(ProtocolKeys.CreateLobbyError, NetEvent.Type.CreateLobbyErr);
        router.RegisterProtocol(ProtocolKeys.JoinLobbyOk, NetEvent.Type.JoinLobbyOk);
        router.RegisterProtocol(ProtocolKeys.JoinLobbyError, NetEvent.Type.JoinLobbyErr);
        router.RegisterProtocol(ProtocolKeys.PlayerJoinLobby, NetEvent.Type.PlayerJoinLobby);
        router.RegisterProtocol(ProtocolKeys.PlayerLeaveLobby, NetEvent.Type.PlayerLeaveLobby);
        router.RegisterProtocol(ProtocolKeys.LobbyNewOwner, NetEvent.Type.LobbyNewOwner);
        router.RegisterProtocol(ProtocolKeys.LobbyReady, NetEvent.Type.LobbyReady);
    }
}