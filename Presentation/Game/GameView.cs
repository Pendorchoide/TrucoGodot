using Godot;

namespace TrucoProject.Presentation {
	public partial class GameView : ViewBase {
		[Export] private Label _gameId;

		CardView _cardView;
		private GameViewModel vm;

		public override void _Ready() {
			vm = ServiceLocator.Get<GameViewModel>();

			vm.GameUpdated += OnGameUpdated;

			vm.CardsRecived += InstantiateCardView;

			vm.EnterGame();
		}

		// ---- Event forwarding (thread-safe) ----
		private void OnGameUpdated() {
			RunOnMainThread(nameof(RefreshUI));
		}

		// ---- UI logic (main thread only) ----
		private void RefreshUI() {
			_gameId.Text = vm.Game.Id;
		}

		// -------- Cleanup --------
		public override void _ExitTree() {
			if (vm == null) return;

			vm.GameUpdated -= OnGameUpdated;
			vm.Dispose();
		}


		public void InstantiateCardView(int rank, int value, string suit) {
			_cardView = GD.Load<PackedScene>("res://Presentation/Card/Card.tscn").Instantiate<CardView>();
			AddChild(_cardView);
			

	
		}
	}
}
