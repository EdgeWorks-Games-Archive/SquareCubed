namespace SquareCubed.Client.Gui.Panels
{
	sealed class MainMenuPanel : GuiPanel
	{
		public MainMenuPanel(SquareCubed.Client.Gui.Gui gui)
			: base(gui, "MainMenu")
		{
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				Gui.Trigger("MainMenu.Dispose");
			}

			base.Dispose(managed);
		}
	}
}
