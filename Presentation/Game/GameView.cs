using Godot;

namespace TrucoProject.Presentation {
	public partial class GameView : ViewBase {
		[Export] private Label _gameId;
		[Export] private PlayerHand _playerHand;
		

		CardView _cardView;
		private GameViewModel vm;

		public override void _Ready() {
			vm = ServiceLocator.Get<GameViewModel>();

			vm.GameUpdated += OnGameUpdated;

			vm.CardsRecived += InstantiateCardView;

			vm.HandDealt += CreateSlots;
			vm.HandDealt += CreateHand;

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
	
		public void CreateSlots(){

			
			var CardSlot1 = GD.Load<PackedScene>("res://Presentation/Hand/HandCardSlot.tscn").Instantiate<Control>();
			var CardSlot2 = GD.Load<PackedScene>("res://Presentation/Hand/HandCardSlot.tscn").Instantiate<Control>();
			var CardSlot3 = GD.Load<PackedScene>("res://Presentation/Hand/HandCardSlot.tscn").Instantiate<Control>();

			
			_playerHand.AddChild(CardSlot1);
			_playerHand.AddChild(CardSlot2);
			_playerHand.AddChild(CardSlot3);
			

		}

		public void CreateHand() {
			CardView _cardView1 = GD.Load<PackedScene>("res://Presentation/Card/Card.tscn").Instantiate<CardView>();
			CardView _cardView2 = GD.Load<PackedScene>("res://Presentation/Card/Card.tscn").Instantiate<CardView>();
			CardView _cardView3 = GD.Load<PackedScene>("res://Presentation/Card/Card.tscn").Instantiate<CardView>();

			_playerHand.GetChild<Control>(0).AddChild(_cardView1);
			_playerHand.GetChild<Control>(1).AddChild(_cardView2);
			_playerHand.GetChild<Control>(2).AddChild(_cardView3);

		}
	}
}
