using Godot;
using TrucoProject.Net.Messages;

namespace TrucoProject.Net.Events.Handlers
{
    public class GameEventsHandler
    {
        public GameEventsHandler()
        {
            NetEventBus.Subscribe(NetEvent.Type.MessageReceived, OnMessageReceived);
        }

        private void OnMessageReceived(NetEvent evt)
        {
            if (evt.Payload is not MessageBase msg)
            {
                GD.PrintErr("[GAME] Payload no es MessageBase");
                return;
            }

            switch (msg)
            {
                case GameStateMessage gsm:
                    GD.Print($"[GAME] Nuevo estado recibido. Turno: {gsm.Turn}");
                    HandleGameState(gsm);
                    break;

                case PlayerJoinedMessage pjm:
                    GD.Print($"[GAME] Player Joined: {pjm.PlayerId}");
                    HandlePlayerJoined(pjm);
                    break;

                case PlayCardResultMessage pcm:
                    GD.Print($"[GAME] PlayCard result for {pcm.PlayerId}");
                    HandlePlayCardResult(pcm);
                    break;

                default:
                    GD.Print("[GAME] Mensaje no reconocido: ", msg.GetType().Name);
                    break;
            }
        }

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
    }
}
