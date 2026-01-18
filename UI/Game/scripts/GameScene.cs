using Godot;
using System;

public partial class GameScene : Node {
	[Export] private Label _gameId;

	Game game;

	public override void _Ready() {
		string gameId = (string) SceneManager.GetInstance().Get("MessagePreviousScene");
		CreateGame(gameId);

		CallDeferred(nameof(UpdateGameId));
	}
	private void UpdateGameId() {
		_gameId.Text = "Codigo: " + game.Id;
	}

	private void CreateGame(string id) {
		game = new Game(id);
	}
}
