using Godot;
using TrucoProject.Net.Events;
using TrucoProject.Net.Messages;

public partial class MainMenu : Control
{
	[Export] private Button _createRoom;

	[Export] private Button _joinRoom;

	[Export] private Button _quit;


	public override void _Ready() {
		_createRoom.Pressed += () => {
			SceneManager.GetInstance().ChangeScene(ScenesPaths.CreateLobby);
		};

		_joinRoom.Pressed += () => SceneManager.GetInstance().ChangeScene(ScenesPaths.JoinLobby);
		
		_quit.Pressed += () => GetTree().Quit();
	}
}