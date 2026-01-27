using System.Collections.Generic;

public class Lobby {
	public readonly string Id;
    public byte MaxPlayers;
	public string OwnerId;
	public Dictionary<string, Player> Players = new();

    public Lobby(string id, byte maxPlayers, string ownerId) {
        Id = id;
        MaxPlayers = maxPlayers;
        OwnerId = ownerId;
    }

    public void AddPlayer(Player player) {
        Players[player.Id] = player;
    }

    public void RemovePlayer(string playerId) {
        Players.Remove(playerId);
    }

    public bool IsReady() {
        return Players.Count == MaxPlayers;
    }
}