using System;
using System.Text.Json;
using Godot;
using TrucoProject.Net.Events;
using TrucoProject.Net.Messages;

public class Player {
		public string Id;
		public string Name;

		private static Player Instance;

		public static Player GetInstance() {
			if (Instance == null) {
				Instance = new Player();
			}

			return Instance;
		}

		private Player() {
            NetEventBus.Subscribe(NetEvent.Type.ConnectionOk, OnConnectionOk);
		}

		private void OnConnectionOk(NetEvent evt) {
			if (evt.Payload is not ConnectionOkMessage msg) return;

			Instance.Id = msg.PlayerId;
        	Instance.Name = msg.PlayerName;
        }

		// for Player Factory...
		private Player(string id, string name) {
			Id = id;
			Name = name;
		}

		public static Player ToPlayer(string raw) {
			try {
				using var doc = JsonDocument.Parse(raw);
				var root = doc.RootElement;

				if (!root.TryGetProperty("id", out var id)) {
					return null;
				}
				
				if (!root.TryGetProperty("name", out var name)) {
					return null;
				}

				return new Player(id.ToString(), name.ToString());
			} catch (Exception e) {
				GD.PrintErr("Error parseando JSON: ", e.Message);
				return null;
			}
		}
	}