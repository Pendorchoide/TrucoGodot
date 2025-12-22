using Godot;
using TrucoProject.Net.Messages;

namespace TrucoProject.Net.Events.Handlers
{
    public class GameEventsHandler {
        public GameEventsHandler() {
            NetEventBus.Subscribe(NetEvent.Type.MessageReceived, OnMessageReceived);
        }

        private void OnMessageReceived(NetEvent evt) {
            if (evt.Payload is not MessageBase msg) {
                GD.PrintErr("[GAME] Payload no es MessageBase");
                return;
            }

            switch (msg) {
                case CreateRoomResultMessage res:
                    HandleCreateRoomResult(res);
                    break;
                /*
                case GameStateMessage gsm:
                    GD.Print($"[GAME] Nuevo estado recibido. Turno: {gsm.Turn}");
                    HandleGameState(gsm);
                    break;
                */
                default:
                    GD.Print("[GAME] Mensaje no reconocido: ", msg.GetType().Name);
                    break;
            }
        }

        private void HandleCreateRoomResult(CreateRoomResultMessage msg) {
            // NetEventBus.Emit(new NetEvent(NetEvent.Type.CreateRoomResult, msg));
        }

        /*
        private void HandleGameState(GameStateMessage msg)
        {
            // TODO: actualizar UI, escena, turnos...
        }

        private void HandlePlayerJoined(PlayerJoinedMessage msg)
        {
            // TODO: agregar jugador al lobby
        }

        private void HandlePlayCardResult(PlayCardResultMessage msg)
        {
            // TODO: animación, validar jugada, actualizar mano
        }
        */
    }
}
