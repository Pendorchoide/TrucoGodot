namespace TrucoProject.Application.Common.Navigation {
    public enum LobbyEntryMode {
        Create,
        Join
    }

    public class NavigationRequest {
        public LobbyEntryMode Mode { get; }
        public string Payload { get; }

        public NavigationRequest(LobbyEntryMode mode, string payload = null) {
            Mode = mode;
            Payload = payload;
        }
    }

}
