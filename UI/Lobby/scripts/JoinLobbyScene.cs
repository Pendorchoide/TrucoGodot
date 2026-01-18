using Godot;

public partial class JoinLobbyScene : Node {
	[Export] private LineEdit _lobbyId;
	[Export] private Button _joinButton;

	public override void _Ready() {
		_joinButton.Pressed += () => {
			SceneManager.GetInstance().ChangeScene(ScenesPaths.Lobby, _lobbyId.Text);
		};
	}
}
