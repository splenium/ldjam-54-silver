using Godot;
using System;

public partial class StartingMenu : Control
{
	[Export]
	private PackedScene StartingScene;

	[Export]
	private Control CreditsControl;
	[Export]
	private BoxContainer Box;
	public void _on_start_button_pressed()
	{
		GetTree().ChangeSceneToFile(StartingScene.ResourcePath);
	}

	private void _on_credits_button_pressed()
	{
		CreditsControl.Visible = true;
		Box.Visible = false;

	}
	private void _on_back_button_pressed()
	{
		CreditsControl.Visible = false;
		Box.Visible = true;

	}
}















