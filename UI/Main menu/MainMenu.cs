using Godot;

public partial class MainMenu : Control
{

    [Export]
    private Button _createRoom;

    [Export]
    private Button _joinRoom;

    [Export]
    private Button _quit;

    public override void _Ready() {
        _createRoom.Pressed += () => GetTree().ChangeSceneToFile("res://UI/Lobby/lobby.tscn");
        _joinRoom.Pressed += () => GD.Print("Botón 2 presionado");
        _quit.Pressed += () => GetTree().Quit();
    }
}