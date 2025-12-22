using Godot;
using System;
using TrucoProject.Net.Events;
using TrucoProject.Net.Messages;
using TrucoProject.Net.WebSocket;

public partial class Lobby : Node {
	[Export]
    private Label _roomId;
	[Export] 
	private Button _goBack;

	public override void _Ready() {
		ConnectAndCreateRoom();
		// NetEventBus.Subscribe(NetEvent.Type.CreateRoomResult, OnCreatedRoom);
		_goBack.Pressed += () => OnGoBack();
	}

	private async void OnGoBack() {
		GetTree().ChangeSceneToFile("res://UI/Main menu/main_menu.tscn");
        await WebSocketClient.Instance.DisconnectAsync();
    }

	private static async void ConnectAndCreateRoom() {
        try {
            await WebSocketClient.Instance.ConnectAsync(new WebSocketConfig("ws://127.0.0.1:8080"));
			WebSocketClient.Instance.Send(new createRoomMessage());
        }
        catch (Exception e) {
            GD.PrintErr(e.Message);
        }
    }

	private void OnCreatedRoom(NetEvent evt) {
		if (evt.Payload is not CreateRoomResultMessage msg) return;

		CallDeferred(nameof(UpdateRoomId), msg.Rooom);
    }

	 private void UpdateRoomId(string id) {
        _roomId.Text = "Codigo: " + id;
    }
}
