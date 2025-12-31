using Godot;
using TrucoProject.Net.Messages;
using TrucoProject.Net.WebSocket;

namespace TrucoProject.Net.Events.Handlers
{
    public class LobbyEventsHandler {
        public LobbyEventsHandler() {
            NetEventBus.Subscribe(NetEvent.Type.CreateLobby, OnCreateLobby);
            NetEventBus.Subscribe(NetEvent.Type.CreateLobbyErr, OnCreateLobbyErr);
            NetEventBus.Subscribe(NetEvent.Type.CreateLobbyOk, OnCreateLobbyOk);
            NetEventBus.Subscribe(NetEvent.Type.JoinLobby, OnJoinLobby);
            NetEventBus.Subscribe(NetEvent.Type.JoinLobbyErr, OnJoinLobbyErr);
            NetEventBus.Subscribe(NetEvent.Type.JoinLobbyOk, OnJoinLobbyOk);
            NetEventBus.Subscribe(NetEvent.Type.PlayerJoinLobby, OnPlayerJoinLobby);
            NetEventBus.Subscribe(NetEvent.Type.LeaveLobby, OnLeaveLobby);
            NetEventBus.Subscribe(NetEvent.Type.PlayerLeaveLobby, OnPlayerLeaveLobby);
            NetEventBus.Subscribe(NetEvent.Type.LobbyNewOwner, OnLobbyNewOwner);
            NetEventBus.Subscribe(NetEvent.Type.LobbyReady, OnLobbyReady);
        }

        private void OnCreateLobby (NetEvent evt) { 
            WebSocketClient.GetInstance().Send((CreateLobbyMessage) evt.Payload);
        }
        private void OnCreateLobbyErr (NetEvent evt) {}
        private void OnCreateLobbyOk (NetEvent evt) {}
        private void OnJoinLobby (NetEvent evt) {}
        private void OnJoinLobbyErr (NetEvent evt) {}
        private void OnJoinLobbyOk (NetEvent evt) {}
        private void OnPlayerJoinLobby (NetEvent evt) {}
        private void OnLeaveLobby (NetEvent evt) {}
        private void OnPlayerLeaveLobby (NetEvent evt) {}
        private void OnLobbyNewOwner (NetEvent evt) {}
        private void OnLobbyReady (NetEvent evt) {}
    }
}
