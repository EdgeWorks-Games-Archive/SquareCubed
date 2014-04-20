namespace SquareCubed.Client.Gui.Panels
{
	public sealed class MainMenuPanel : GuiPanel
	{
		public MainMenuPanel(Gui gui)
			: base(gui, "MainMenu")
		{
		}

		public void Show()
		{
			Gui.Trigger("MainMenu.Show");
		}

		public void Hide()
		{
			Gui.Trigger("MainMenu.Hide");
		}
	}
}
