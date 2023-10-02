using Godot;
using System;

public partial class StartingMenu : Control
{
	[Export]
	private PackedScene StartingScene;

	[Export]
	private Control CreditsControl;
	[Export]
	private BoxContainer BoxContainer;

	public void _on_start_button_pressed()
	{
		GetTree().ChangeSceneToFile(StartingScene.ResourcePath);
	}

	private void _on_credits_button_pressed()
	{
		CreditsControl.Visible = true;
		BoxContainer.Visible = false;

	}
	private void _on_back_button_pressed()
	{
		CreditsControl.Visible = false;
		BoxContainer.Visible = true;

	}
}















