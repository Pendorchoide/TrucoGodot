namespace TrucoProject.Application.Common.Navigation {

    public interface INavigationService {

        // ───────────── App flow ─────────────

        void NavigateToLogin();
        void NavigateToMainMenu();

        void NavigateToCreateMatchScreen();
        
        void NavigateToJoinMatchScreen();

        // ───────────── Lobby entry intent ─────────────

        void NavigateToCreateMatch(int maxPlayers);
        void NavigateToJoinMatch(string lobbyId);

        // ───────────── Game flow ─────────────

        void NavigateToGame(string gameId);

        // ───────────── State ─────────────

        NavigationRequest ConsumeLastNavigation();

        // ───────────── System ─────────────

        void Quit();
    }
}
