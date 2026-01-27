using TrucoProject.Application.Common.Navigation;

public class MainMenuViewModel {

    private readonly INavigationService navigation;

    public MainMenuViewModel(INavigationService navigation) {
        this.navigation = navigation;
    }

    public void CreateMatch() {
        navigation.NavigateToCreateMatchScreen();
    }

    public void JoinMatch() {
        navigation.NavigateToJoinMatchScreen();
    }

    public void Quit() {
        navigation.Quit();
    }
}