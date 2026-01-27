using System;
using TrucoProject.Application.Common.Navigation;

public class LoginViewModel {
    private readonly AuthService auth;
    private readonly INavigationService navigation;

    public event Action LoginStarted;
    public event Action LoginSucceeded;
    public event Action<string> LoginFailed;

    public LoginViewModel(
        AuthService auth,
        INavigationService navigation
    ) {
        this.auth = auth;
        this.navigation = navigation;
    }

    public async void Login(string userId) {
        if (string.IsNullOrWhiteSpace(userId)) {
            LoginFailed?.Invoke("User vacío");
            return;
        }

        try {
            LoginStarted?.Invoke();

            await auth.Connect(userId);

            // Estado global (luego AppState)
            Player.GetInstance();

            LoginSucceeded?.Invoke();

            navigation.NavigateToMainMenu();
        }
        catch (Exception e) {
            LoginFailed?.Invoke(e.Message);
        }
    }
}
