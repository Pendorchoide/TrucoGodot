using System.Threading.Tasks;
using TrucoProject.Net.Messages;
using TrucoProject.Net.Events;
using Godot;
using TrucoProject.Net.WebSocket;

namespace TrucoProject.Net.Utils
{
    public class Heartbeat {
        private static Heartbeat _instance { get; set; }
        public int _intervalSeconds { get; set; }

        private bool _running = true;
        private bool _waitingForPong = false;

        public static Heartbeat GetInstance() {
            if (_instance == null) {
                _instance = new Heartbeat();
            }

            return _instance;
        }

        private Heartbeat() {
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

                    WebSocketClient.GetInstance().Send(new PingMessage());
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
