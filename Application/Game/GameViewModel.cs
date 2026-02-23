using System;
using TrucoProject.Application.Common.Navigation;

public class GameViewModel {
    public Game Game { get; private set; }

    public event Action GameUpdated;

    private readonly INavigationService navigation;

    public event Action<int, int, string> CardsRecived;
    public event Action HandDealt;





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
        //CardsRecived?.Invoke(1,1,"Oro");
        HandDealt?.Invoke();
        
    }

    // ───────────── Net subscriptions ─────────────
    public void Subscribe() {}
    public void Dispose() {}

    // ───────────── Handlers ─────────────
    // ...

    private void CreateCard(int rank, int value, string suit) {
        Game.Card = new Card(rank, value, suit);

    }

}
