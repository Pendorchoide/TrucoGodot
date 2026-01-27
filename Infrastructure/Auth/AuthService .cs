using System.Threading.Tasks;
using TrucoProject.Net.WebSocket;

public class AuthService {

    private readonly WebSocketClient ws;

    public AuthService(WebSocketClient ws) {
        this.ws = ws;
    }

    public async Task Connect(string userId) {
        string url = $"ws://127.0.0.1:8080/?at={userId}";
        await ws.ConnectAsync(new WebSocketConfig(url));
    }
}