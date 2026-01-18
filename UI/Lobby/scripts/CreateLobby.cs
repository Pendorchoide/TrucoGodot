using Godot;
using System;

public partial class CreateLobby : Node {
	[Export] private LineEdit _maxPlayers;
	[Export] private Button _createButton;

	public override void _Ready() {
		_createButton.Pressed += () => {
			SceneManager.GetInstance().ChangeScene(ScenesPaths.Lobby, _maxPlayers.Text);
		};
	}
}
