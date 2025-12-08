using Godot;
using TrucoProject.Net.Events.Handlers;

public partial class GameNet : Node
{
    private GameEventsHandler _gameHandler;
    private ConnectionEventsHandler _connectionHandler;

    public override void _Ready()
    {
        _gameHandler = new GameEventsHandler();
        _connectionHandler = new ConnectionEventsHandler();

        GD.Print("[GameNet] Handlers inicializados.");
    }
}