using Godot;

namespace TrucoProject.Presentation {
	public partial class CreateMatchView : ViewBase {
		[Export] private LineEdit _maxPlayers;
		[Export] private Button _create;
		[Export] private Button _back;

		private CreateMatchViewModel vm;

		public override void _Ready() {
			vm = ServiceLocator.Get<CreateMatchViewModel>();

			_create.Pressed += OnCreatePressed;
			_back.Pressed += vm.Back;

			vm.Error += OnError;
		}

		private void OnCreatePressed() {
			vm.CreateMatch(_maxPlayers.Text);
		}

		private void OnError(string msg) {
			GD.PrintErr(msg);
		}

		public override void _ExitTree() {
			if (vm == null) return;

			_create.Pressed -= OnCreatePressed;
			_back.Pressed -= vm.Back;

			vm.Error -= OnError;
		}
	}
}