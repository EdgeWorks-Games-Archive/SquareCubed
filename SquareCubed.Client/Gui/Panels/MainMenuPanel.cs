namespace SquareCubed.Client.Gui.Panels
{
	public sealed class MainMenuPanel : GuiPanel
	{
		public MainMenuPanel(OldGui oldGui)
			: base(oldGui, "MainMenu")
		{
		}

		public void Show()
		{
			OldGui.Trigger("MainMenu.Show");
		}

		public void Hide()
		{
			OldGui.Trigger("MainMenu.Hide");
		}
	}
}
