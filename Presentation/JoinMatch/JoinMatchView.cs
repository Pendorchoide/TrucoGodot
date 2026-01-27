using Godot;

namespace TrucoProject.Presentation {
	public partial class JoinMatchView : ViewBase {
		[Export] private LineEdit _lobbyId;
		[Export] private Button _join;
		[Export] private Button _back;

		private JoinMatchViewModel vm;

		public override void _Ready() {
			vm = ServiceLocator.Get<JoinMatchViewModel>();

			_join.Pressed += OnJoinPressed;
			_back.Pressed += vm.Back;

			vm.Error += OnError;
		}

		private void OnJoinPressed() {
			vm.JoinMatch(_lobbyId.Text);
		}

		private void OnError(string msg) {
			GD.PrintErr(msg);
		}

		public override void _ExitTree() {
			if (vm == null) return;

			_join.Pressed -= OnJoinPressed;
			_back.Pressed -= vm.Back;

			vm.Error -= OnError;
		}
	}
}