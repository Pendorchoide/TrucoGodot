using Godot;
using System;

public partial class CheatHelper : Node
{
	[Export] private LineEdit lineEdit;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lineEdit.Visible = false;
		lineEdit.Clear();
		lineEdit.
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (lineEdit.Visible && Input.IsActionJustPressed("ui_accept"))
		{
			ProcessCheat(lineEdit.Text);
			lineEdit.Text = "";
			lineEdit.Visible = false;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent )
		{
			if (keyEvent.Pressed && keyEvent.Keycode == Key.T && !lineEdit.IsEditing())
			{
				lineEdit.Visible = !lineEdit.Visible;
				if (lineEdit.Visible)
					lineEdit.Edit();
				lineEdit.Clear();
			}
			
		}
	}

	public void ProcessCheat(string cheat)
	{
		var parts = cheat.Split(' ');
		
		if (parts.Length == 0) return;

		switch (parts[0].ToLower())
		{
			case "help":
				GD.Print("Cheat commands:");
				GD.Print("givecard <rank> <value> <suit> - Gives a card to the player");
				break;
			case "givecard":
				if (parts.Length < 4) return;
				if (int.TryParse(parts[1], out int rank) && int.TryParse(parts[2], out int value))
				{
					string suit = parts[3];
					GD.Print($"Cheat: Giving card {rank} of {suit} with value {value}");
					// Aquí podrías llamar a un método en tu GameViewModel para agregar la carta al juego
				}
				else
				{
					GD.Print("Cheat: Invalid rank or value");
				}
				break;

			default:
				GD.Print("Cheat: Unknown command");
				break;
		}
	}
}
