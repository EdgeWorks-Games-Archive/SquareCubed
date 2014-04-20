namespace SquareCubed.Client.Gui.Panels
{
	sealed class EscMenuPanel : GuiPanel
	{
		public EscMenuPanel(SquareCubed.Client.Gui.Gui gui)
			: base(gui, "EscMenu")
		{
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				Gui.Trigger("EscMenu.Dispose");
			}

			base.Dispose(managed);
		}
	}
}