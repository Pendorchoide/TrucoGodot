using System;
using TrucoProject.Application.Common.Navigation;

public class GameViewModel {
    public Game Game { get; private set; }

    public event Action GameUpdated;

    private readonly INavigationService navigation;

    public GameViewModel(INavigationService navigation) {
        this.navigation = navigation;
    }

    // ───────────── User actions ─────────────
    public void EnterGame() {
        Subscribe();

        var nav = navigation.ConsumeLastNavigation();
        if (nav == null) return;

        Game = new Game(nav.Payload);
        GameUpdated?.Invoke(); // 🔑 el VM notifica
    }

    // ───────────── Net subscriptions ─────────────
    public void Subscribe() {}
    public void Dispose() {}

    // ───────────── Handlers ─────────────
    // ...
}
