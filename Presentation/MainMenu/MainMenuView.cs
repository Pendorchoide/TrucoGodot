using Godot;

namespace TrucoProject.Presentation {
	public partial class MainMenuView : ViewBase {
		[Export] private Button _create;
		[Export] private Button _join;
		[Export] private Button _quit;

		private MainMenuViewModel vm;

		public override void _Ready() {
			vm = ServiceLocator.Get<MainMenuViewModel>();

			_create.Pressed += vm.CreateMatch;
			_join.Pressed += vm.JoinMatch;
			_quit.Pressed += vm.Quit;
		}

		public override void _ExitTree() {
			if (vm == null) return;

			_create.Pressed -= vm.CreateMatch;
			_join.Pressed -= vm.JoinMatch;
			_quit.Pressed -= vm.Quit;
		}
	}
}
