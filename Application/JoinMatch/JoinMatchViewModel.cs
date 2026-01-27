using System;
using TrucoProject.Application.Common.Navigation;

public class JoinMatchViewModel {
    private readonly INavigationService navigation;
    public event Action<string> Error;

    public JoinMatchViewModel(INavigationService navigation) {
        this.navigation = navigation;
    }

    public void JoinMatch(string lobbyId) {
        if (string.IsNullOrWhiteSpace(lobbyId)) {
            Error?.Invoke("Código inválido");
            return;
        }

        navigation.NavigateToJoinMatch(lobbyId.Trim());
    }

    public void Back() {
        navigation.NavigateToMainMenu();
    }
}