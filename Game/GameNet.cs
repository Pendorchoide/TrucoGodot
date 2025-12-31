using Godot;
using TrucoProject.Net.Events.Handlers;
using TrucoProject.Net.WebSocket;

public partial class GameNet : Node {
    private ConnectionEventsHandler _connectionHandler;

    private LobbyEventsHandler _lobbyHandler;
    private GameEventsHandler _gameHandler;

    public override void _Ready() {
        WebSocketClient.GetInstance();
        ProtocolRoutes.RegisterAll(MessageRouter.GetInstance());

        _gameHandler = new GameEventsHandler();
        _connectionHandler = new ConnectionEventsHandler();
        _lobbyHandler = new LobbyEventsHandler();

        GD.Print("[GameNet] Handlers inicializados.");
    }
}