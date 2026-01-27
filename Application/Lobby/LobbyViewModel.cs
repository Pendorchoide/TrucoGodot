using System;
using System.Collections.Generic;
using TrucoProject.Application.Common.Navigation;
using TrucoProject.Net.Events;
using TrucoProject.Net.Messages;

public class LobbyViewModel {
    private readonly INavigationService navigation;

    public Lobby Lobby { get; private set; }

    public event Action LobbyUpdated;
    public event Action<bool> StartAvailabilityChanged;

    public LobbyViewModel(INavigationService navigation) {
        this.navigation = navigation;
    }

    // ───────────── User actions ─────────────

    public void EnterLobby() {
        Subscribe();

        var nav = navigation.ConsumeLastNavigation();
        if (nav == null) return;

        if (nav.Mode == LobbyEntryMode.Create) {
            NetEventBus.Emit(new NetEvent(
                NetEvent.Type.CreateLobby,
                new CreateLobbyMessage(int.Parse(nav.Payload))
            ));
        } else {
            NetEventBus.Emit(new NetEvent(
                NetEvent.Type.JoinLobby,
                new JoinLobbyMessage(nav.Payload)
            ));
        }
    }

    public void LeaveLobby() {
        NetEventBus.Emit(new NetEvent(
            NetEvent.Type.LeaveLobby,
            new LeaveLobbyMessage()
        ));

        Dispose();
        navigation.NavigateToMainMenu();
    }

    public void StartGame() {
        if (Lobby == null) return;

        NetEventBus.Emit(new NetEvent(
            NetEvent.Type.LobbyReady,
            new LobbyReadyMessage(Lobby.Id)
        ));
    }

    private void OnGameStarted(NetEvent evt) {
        if (evt.Payload is not GameStartedMessage msg) return;

        Dispose();
        navigation.NavigateToGame(msg.GameId);
    }


    // ───────────── Net subscriptions ─────────────

    public void Subscribe() {
        NetEventBus.Subscribe(NetEvent.Type.CreateLobbyOk, OnCreateLobbyOk);
        NetEventBus.Subscribe(NetEvent.Type.JoinLobbyOk, OnJoinLobbyOk);
        NetEventBus.Subscribe(NetEvent.Type.PlayerJoinLobby, OnPlayerJoinLobby);
        NetEventBus.Subscribe(NetEvent.Type.PlayerLeaveLobby, OnPlayerLeftLobby);
        NetEventBus.Subscribe(NetEvent.Type.LobbyNewOwner, OnLobbyNewOwner);
        NetEventBus.Subscribe(NetEvent.Type.GameStarted, OnGameStarted);
    }

    public void Dispose() {
        NetEventBus.Unsubscribe(NetEvent.Type.CreateLobbyOk, OnCreateLobbyOk);
        NetEventBus.Unsubscribe(NetEvent.Type.JoinLobbyOk, OnJoinLobbyOk);
        NetEventBus.Unsubscribe(NetEvent.Type.PlayerJoinLobby, OnPlayerJoinLobby);
        NetEventBus.Unsubscribe(NetEvent.Type.PlayerLeaveLobby, OnPlayerLeftLobby);
        NetEventBus.Unsubscribe(NetEvent.Type.LobbyNewOwner, OnLobbyNewOwner);
        NetEventBus.Unsubscribe(NetEvent.Type.GameStarted, OnGameStarted);
    }

    // ───────────── Handlers ─────────────

    private void CreateLobby(string id, byte maxPlayers, string ownerId, List<string> players) {
        Lobby = new Lobby(id, maxPlayers, ownerId);

        players.ForEach(p => Lobby.AddPlayer(Player.ToPlayer(p)));

        LobbyUpdated?.Invoke();
        StartAvailabilityChanged?.Invoke(Lobby.IsReady());
    }

    private void OnCreateLobbyOk(NetEvent evt) {
        if (evt.Payload is not CreateLobbyOkMessage msg) return;
        CreateLobby(msg.LobbyId, msg.MaxPlayers, msg.Owner, msg.Players);
    }

    private void OnJoinLobbyOk(NetEvent evt) {
        if (evt.Payload is not JoinLobbyOkMessage msg) return;
        CreateLobby(msg.LobbyId, msg.MaxPlayers, msg.Owner, msg.Players);
    }

    private void OnPlayerJoinLobby(NetEvent evt) {
        if (evt.Payload is not PlayerJoinLobbyMessage msg) return;

        Lobby.AddPlayer(Player.ToPlayer(
            $"{{ \"id\": \"{msg.PlayerId}\", \"name\": \"{msg.PlayerName}\" }}"
        ));

        StartAvailabilityChanged?.Invoke(Lobby.IsReady());
        LobbyUpdated?.Invoke();
    }

    private void OnPlayerLeftLobby(NetEvent evt) {
        if (evt.Payload is not PlayerLeaveLoobyMessage msg) return;

        Lobby.RemovePlayer(msg.PlayerId);
        StartAvailabilityChanged?.Invoke(Lobby.IsReady());
        LobbyUpdated?.Invoke();
    }

    private void OnLobbyNewOwner(NetEvent evt) {
        if (evt.Payload is not LobbyNewOwnerMessage msg) return;

        Lobby.OwnerId = msg.NewOwner;
        LobbyUpdated?.Invoke();
    }
}