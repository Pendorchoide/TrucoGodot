using TrucoProject.Application.Common.Navigation;

namespace TrucoProject.Infrastructure.Navigation {
    public class NavigationService : INavigationService {

        private readonly SceneManager sceneManager;
        private NavigationRequest lastNavigation;

        public NavigationService(SceneManager sceneManager) {
            this.sceneManager = sceneManager;
        }

        public void NavigateToCreateMatch(int maxPlayers) {
            lastNavigation = new NavigationRequest(
                LobbyEntryMode.Create,
                maxPlayers.ToString()
            );

            sceneManager.ChangeScene(ScenesPaths.Lobby);
        }

        public void NavigateToJoinMatch(string lobbyId) {
            lastNavigation = new NavigationRequest(
                LobbyEntryMode.Join,
                lobbyId
            );
            sceneManager.ChangeScene(ScenesPaths.Lobby);
        }

        public NavigationRequest ConsumeLastNavigation() {
            var nav = lastNavigation;
            lastNavigation = null;
            return nav;
        }

        
        public void NavigateToMainMenu () =>
            sceneManager.ChangeScene(ScenesPaths.MainMenu);

        public void NavigateToCreateMatchScreen () =>
            sceneManager.ChangeScene(ScenesPaths.CreateLobby);

        public void NavigateToJoinMatchScreen () =>
            sceneManager.ChangeScene(ScenesPaths.JoinLobby);

        public void NavigateToLogin () =>
            sceneManager.ChangeScene(ScenesPaths.Login);

        public void NavigateToGame (string gameId) {
            lastNavigation = new NavigationRequest(
                LobbyEntryMode.Create,
                gameId
            );

            sceneManager.ChangeScene(ScenesPaths.Game);
        }

        public void Quit () => sceneManager.Quit();
    }
}