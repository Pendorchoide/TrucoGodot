using System;
using TrucoProject.Application.Common.Navigation;

public class CreateMatchViewModel {
    private readonly INavigationService navigation;

    public event Action<string> Error;

    public CreateMatchViewModel(INavigationService navigation) {
        this.navigation = navigation;
    }

    public void CreateMatch(string maxPlayersText) {
        if (!int.TryParse(maxPlayersText, out var maxPlayers) || maxPlayers <= 0) {
            Error?.Invoke("Cantidad inválida");
            return;
        }

        navigation.NavigateToCreateMatch(maxPlayers);
    }

    public void Back() {
        navigation.NavigateToMainMenu();
    }
}