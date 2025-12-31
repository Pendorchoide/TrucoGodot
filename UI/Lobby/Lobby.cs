using Godot;
using System;
using TrucoProject.Net.Events;
using TrucoProject.Net.Messages;
using TrucoProject.Net.WebSocket;

public partial class Lobby : Node {
	[Export]
	private Label _lobbyId;
	[Export] 
	private Button _goBack;

	public override void _Ready() {
		ConnectAndCreateLobby();

		NetEventBus.Subscribe(NetEvent.Type.CreateLobbyOk, OnCreateLobbyOk);
		_goBack.Pressed += () => OnGoBack();
	}

	private async void OnGoBack() {
		GetTree().ChangeSceneToFile("res://UI/Main menu/main_menu.tscn");
		await WebSocketClient.GetInstance().DisconnectAsync();
		NetEventBus.Unsubscribe(NetEvent.Type.CreateLobbyOk, OnCreateLobbyOk);
	}

	private static async void ConnectAndCreateLobby() {
		try {
			await WebSocketClient.GetInstance()
				.ConnectAsync(new WebSocketConfig("ws://127.0.0.1:8080/?at=1"));
			
			NetEventBus.Emit(new NetEvent(
				NetEvent.Type.CreateLobby,
				new CreateLobbyMessage(2)
			));
		}

		catch (Exception e) {
			GD.PrintErr(e.Message);
		}
	}

	private void OnCreateLobbyOk(NetEvent evt) {
		if (evt.Payload is not CreateLobbyOkMessage msg) return;

		CallDeferred(nameof(UpdateLobbyId), msg.LobbyId);
	}

	 private void UpdateLobbyId(string id) {
		_lobbyId.Text = "Codigo: " + id;
	}
}
