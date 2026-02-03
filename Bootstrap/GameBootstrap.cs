using Godot;
using TrucoProject.Application.Common.Navigation;
using TrucoProject.Infrastructure.Navigation;
using TrucoProject.Net.WebSocket;

public partial class GameBootstrap : Node
{
	public override void _Ready() {
		RegisterServices();
		CallDeferred(nameof(StartApplication));
	}

	private void StartApplication() {
		ServiceLocator
			.Get<INavigationService>()
			.NavigateToGame("pichula al horno");
	}

	private void RegisterServices()
	{
		// ───────────── Core / Network ─────────────

		var ws = WebSocketClient.GetInstance();
		ServiceLocator.Register(ws);

		var auth = new AuthService(ws);
		ServiceLocator.Register(auth);

		// ───────────── Navigation ─────────────

		var sceneManager = SceneManager.GetInstance();
		var navigation = new NavigationService(sceneManager);

		ServiceLocator.Register<INavigationService>(navigation);

		// ───────────── ViewModels ─────────────

		ServiceLocator.Register(new LoginViewModel(auth, navigation));
		ServiceLocator.Register(new MainMenuViewModel(navigation));
		ServiceLocator.Register(new CreateMatchViewModel(navigation));
		ServiceLocator.Register(new JoinMatchViewModel(navigation));
		ServiceLocator.Register(new LobbyViewModel(navigation));
		ServiceLocator.Register(new GameViewModel(navigation));
		
	}
}