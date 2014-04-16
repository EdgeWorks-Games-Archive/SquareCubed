using SquareCubed.Client.Gui;

namespace SquareCubed.Client.MainMenu
{
	sealed class MainMenuPanel : GuiPanel
	{
		public MainMenuPanel(Gui.Gui gui)
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
