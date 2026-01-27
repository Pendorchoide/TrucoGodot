using Godot;

namespace TrucoProject.Presentation {
    public partial class LobbyView : ViewBase {
        [Export] private Label _lobbyId;
        [Export] private Button _goBack;
        [Export] private Button _startGame;
        [Export] private ItemList _playersList;

        private LobbyViewModel vm;

        public override void _Ready() {
            vm = ServiceLocator.Get<LobbyViewModel>();

            vm.EnterLobby();

            _goBack.Pressed += vm.LeaveLobby;
            _startGame.Pressed += vm.StartGame;

            vm.LobbyUpdated += OnLobbyUpdated;
            vm.StartAvailabilityChanged += OnStartAvailabilityChanged;
        }

        // ───────────── Event forwarding ─────────────

        private void OnLobbyUpdated() {
            RunOnMainThread(nameof(RefreshUI));
        }

        private void OnStartAvailabilityChanged(bool canStart) {
            RunOnMainThread(nameof(UpdateStartAvailability), canStart);
        }

        // ───────────── UI logic ─────────────

        private void RefreshUI() {
            if (vm.Lobby == null) return;

            _lobbyId.Text = $"Código: {vm.Lobby.Id}";
            _playersList.Clear();

            foreach (var p in vm.Lobby.Players.Values)
            {
                string icon = vm.Lobby.OwnerId == p.Id ? "👑" : "🃏";
                _playersList.AddItem($"{icon} {p.Name}");
            }

            _startGame.Visible = vm.Lobby.OwnerId == Player.GetInstance().Id;
        }

        private void UpdateStartAvailability(bool canStart) {
            _startGame.Disabled = !canStart;
        }

        // ───────────── Cleanup ─────────────

        public override void _ExitTree() {
            vm.LobbyUpdated -= OnLobbyUpdated;
            vm.StartAvailabilityChanged -= OnStartAvailabilityChanged;

            _goBack.Pressed -= vm.LeaveLobby;
            _startGame.Pressed -= vm.StartGame;

            vm.Dispose();
        }
    }
}