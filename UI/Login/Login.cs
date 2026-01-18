using System;
using Godot;
using TrucoProject.Net.WebSocket;

public partial class Login : Node {
    [Export] private LineEdit _userId;
	[Export] private Button _login;

    public override void _Ready() {

        _login.Pressed += () => {
            if (_userId.Text == "") return;

            ConnectToServer(_userId.Text);
            Player.GetInstance();
            SceneManager.GetInstance().ChangeScene(ScenesPaths.MainMenu);
        };
    }
    
    private async static void ConnectToServer(string id) {
        string url = "ws://127.0.0.1:8080/?at=" + id;

		try {
			await WebSocketClient
                .GetInstance()
				.ConnectAsync(new WebSocketConfig(url));
		}
		catch (Exception e) {
			GD.PrintErr(e.Message);
		}
	}
}