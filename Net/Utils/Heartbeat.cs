using System.Threading.Tasks;
using TrucoProject.Net.Messages;
using TrucoProject.Net.Events;
using Godot;

namespace TrucoProject.Net.Utils
{
    public class Heartbeat {
        private readonly WebSocket.WebSocketClient _client;
        private readonly int _intervalSeconds;

        private bool _running = true;
        private bool _waitingForPong = false;

        public Heartbeat(WebSocket.WebSocketClient client, int intervalSeconds) {
            _client = client;
            _intervalSeconds = intervalSeconds;

            StartLoop();
        }

        private void StartLoop() {
            Task.Run (async () => {
                while (_running) {
                    await Task.Delay(_intervalSeconds * 1000);

                    if (!_running) {
                        break;
                    }

                    // If last ping wasn't answered
                    if (_waitingForPong) {
                        NetLogger.Warn("[Heartbeat] No pong received → connection lost?");
                        NetEventBus.Emit(new NetEvent(NetEvent.Type.Disconnected));
                        return;
                    }

                    // Send ping
                    _client.Send(new PingMessage());
                    _waitingForPong = true;
                }
            });
        }

        public void OnPong() {
            _waitingForPong = false;
        }

        public void Stop() {
            _running = false;
        }
    }
}
