using System.Collections.Generic;
using Godot;
using TrucoProject.Net.Events;
using TrucoProject.Net.Messages;

public partial class LobbyScene : Node {
	[Export] private Label _lobbyId;
	[Export] private Button _goBack;
	[Export] private Button _startGame;
	[Export] private ItemList _playersList;

	Lobby lobby;

	public override void _Ready() {
		SuscribeAll();
		ProcessPreviousScene();
		_goBack.Pressed += () => OnGoBack();
		_startGame.Pressed += () => OnStartGame();
	}

	private void OnGoBack() {
		UnsubscribeAll();
		
		NetEventBus.Emit(new NetEvent(
			NetEvent.Type.LeaveLobby,
			new LeaveLobbyMessage()
		));

		SceneManager.GetInstance().ChangeScene(ScenesPaths.MainMenu);
	}

	private void OnStartGame() {
		NetEventBus.Emit(new NetEvent(
			NetEvent.Type.LobbyReady,
			new LobbyReadyMessage(lobby.Id)
		));
	}

	private static void ProcessPreviousScene() {
		string msg = (string) SceneManager.GetInstance().Get("MessagePreviousScene");

		switch ((string) SceneManager.GetInstance().Get("PrevScene")) {
			case ScenesPaths.CreateLobby: 
				int intMsg = int.Parse(msg);

				NetEventBus.Emit(new NetEvent(
					NetEvent.Type.CreateLobby,
					new CreateLobbyMessage(intMsg)
				));
			break;

			case ScenesPaths.JoinLobby: 
				if (msg == null) {
					SceneManager.GetInstance().ChangeScene(ScenesPaths.MainMenu);
				}

				NetEventBus.Emit(new NetEvent(
					NetEvent.Type.JoinLobby,
					new JoinLobbyMessage(msg)
				));
			break;
		}
	}

	private void SuscribeAll() {
		NetEventBus.Subscribe(NetEvent.Type.CreateLobbyErr, OnCreateLobbyErr);
		NetEventBus.Subscribe(NetEvent.Type.CreateLobbyOk, OnCreateLobbyOk);
		NetEventBus.Subscribe(NetEvent.Type.JoinLobbyErr, OnJoinLobbyErr);
		NetEventBus.Subscribe(NetEvent.Type.JoinLobbyOk, OnJoinLobbyOk);
		NetEventBus.Subscribe(NetEvent.Type.PlayerJoinLobby, OnPlayerJoinLobby);
		NetEventBus.Subscribe(NetEvent.Type.PlayerLeaveLobby, OnPlayerLeftLobby);
		NetEventBus.Subscribe(NetEvent.Type.LobbyNewOwner, OnLobbyNewOwner);
		NetEventBus.Subscribe(NetEvent.Type.LobbyReady, OnLobbyReady);

		NetEventBus.Subscribe(NetEvent.Type.GameStarted, OnGameStarted);
	}

	private void UnsubscribeAll() {
		NetEventBus.Unsubscribe(NetEvent.Type.CreateLobbyErr, OnCreateLobbyErr);
		NetEventBus.Unsubscribe(NetEvent.Type.CreateLobbyOk, OnCreateLobbyOk);
		NetEventBus.Unsubscribe(NetEvent.Type.JoinLobbyErr, OnJoinLobbyErr);
		NetEventBus.Unsubscribe(NetEvent.Type.JoinLobbyOk, OnJoinLobbyOk);
		NetEventBus.Unsubscribe(NetEvent.Type.PlayerJoinLobby, OnPlayerJoinLobby);
		NetEventBus.Unsubscribe(NetEvent.Type.PlayerLeaveLobby, OnPlayerLeftLobby);
		NetEventBus.Unsubscribe(NetEvent.Type.LobbyNewOwner, OnLobbyNewOwner);
		NetEventBus.Unsubscribe(NetEvent.Type.LobbyReady, OnLobbyReady);

		NetEventBus.Unsubscribe(NetEvent.Type.GameStarted, OnGameStarted);
	}

	private void UpdateLobbyId() {
		_lobbyId.Text = "Codigo: " + lobby.Id;
	}

	private void DisplayPlayerList(){
		_playersList.Clear();

		foreach (var p in lobby.Players.Values) {

			string displayName = (lobby.OwnerId == p.Id) 
				?  "👑 " + p.Name
				:  "🃏 " + p.Name; 

			_playersList.AddItem(displayName);
		}
	}

	private void DisplayStartButton() {
		_startGame.Visible = true;
	}

	private void ChangeStartButtonAvaility(bool availity) {
		_startGame.Disabled = !availity;
	}

	private void OnCreateLobbyErr(NetEvent evt) {
		// TODO
	}

	private void CreateLobby(string lobbyId, byte maxPlayers, string ownerId, List<string> players) {
		lobby = new Lobby(lobbyId, maxPlayers, ownerId);

		players.ForEach(p => {
			Player player = Player.ToPlayer(p);
			lobby.AddPlayer(player);
		});
	}

	private void OnCreateLobbyOk(NetEvent evt) {
		if (evt.Payload is not CreateLobbyOkMessage msg) return;

		CreateLobby(msg.LobbyId, msg.MaxPlayers, msg.Owner, msg.Players);

		CallDeferred(nameof(UpdateLobbyId));
		CallDeferred(nameof(DisplayPlayerList));
		CallDeferred(nameof(DisplayStartButton));
	}

	private void OnJoinLobbyErr(NetEvent evt) {
		// TODO
	}

	private void OnJoinLobbyOk(NetEvent evt) {
		if (evt.Payload is not JoinLobbyOkMessage msg) return;

		CreateLobby(msg.LobbyId, msg.MaxPlayers, msg.Owner, msg.Players);

		CallDeferred(nameof(DisplayPlayerList));
		CallDeferred(nameof(UpdateLobbyId));
	}

	private void OnPlayerLeftLobby(NetEvent evt) {
		if (evt.Payload is not PlayerLeaveLoobyMessage msg) return;

		lobby.RemovePlayer(msg.PlayerId);
		CallDeferred(nameof(DisplayPlayerList));
	}

	private void OnLobbyNewOwner(NetEvent evt) {
		if (evt.Payload is not LobbyNewOwnerMessage msg) return;
		lobby.OwnerId = msg.NewOwner;

		CallDeferred(nameof(DisplayPlayerList));

		if (lobby.OwnerId == Player.GetInstance().Id) {
			CallDeferred(nameof(DisplayStartButton));
		}
	}

	private void OnLobbyReady(NetEvent evt) {
		// TODO
	}

	private void OnPlayerJoinLobby(NetEvent evt) {
		if (evt.Payload is not PlayerJoinLobbyMessage msg) return;

		Player player = Player.ToPlayer("{ \"id\": \"" + msg.PlayerId + "\", \"name\": \"" + msg.PlayerName + "\" }");

		lobby.AddPlayer(player);

		if (lobby.IsReady()) {
			CallDeferred(nameof(ChangeStartButtonAvaility), true);
		}

		CallDeferred(nameof(DisplayPlayerList));
	}

	private void goToGame(string gameId)
	{
		SceneManager.GetInstance().ChangeScene(ScenesPaths.Game, gameId);
	}

	private void OnGameStarted(NetEvent evt) {
		if (evt.Payload is not GameStartedMessage msg) return;

		UnsubscribeAll();
		CallDeferred(nameof(goToGame), msg.GameId);
	}
}
