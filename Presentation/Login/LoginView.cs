using Godot;

namespace TrucoProject.Presentation {
	public partial class LoginView : ViewBase {
		[Export] private LineEdit _userId;
		[Export] private Button _login;

		private LoginViewModel vm;

		public override void _Ready() {
			vm = ServiceLocator.Get<LoginViewModel>();

			_login.Pressed += OnLoginPressed;

			vm.LoginSucceeded += OnLoginSucceeded;
			vm.LoginFailed += OnLoginFailed;
		}

		private void OnLoginPressed() {
			vm.Login(_userId.Text);
		}

		// ---- Event forwarding (thread-safe) ----

		private void OnLoginSucceeded() {
			RunOnMainThread(nameof(HandleLoginSuccess));
		}

		private void OnLoginFailed(string msg) {
			RunOnMainThread(nameof(HandleLoginFailed), msg);
		}

		// ---- UI logic (main thread only) ----

		private void HandleLoginSuccess() {
			// La navegación YA pasó desde el VM
			// Acá solo UI
			GD.Print("Login OK");
		}

		private void HandleLoginFailed(string msg) {
			GD.PrintErr(msg);
		}

		// -------- Cleanup --------

		public override void _ExitTree() {
			if (vm == null) return;

			vm.LoginSucceeded -= OnLoginSucceeded;
			vm.LoginFailed -= OnLoginFailed;

			_login.Pressed -= OnLoginPressed;
		}
	}
}