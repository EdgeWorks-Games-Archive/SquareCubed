using SquareCubed.Client.Gui;

namespace SQCore.Client.Gui
{
	public class ContextInfoPanel : GuiPanel
	{
		private readonly SquareCubed.Client.Gui.Gui _gui;

		public ContextInfoPanel(SquareCubed.Client.Gui.Gui gui)
			: base(gui, "ContextInfo")
		{
			_gui = gui;
		}

		public bool IsVisible
		{
			set { _gui.Trigger("ContextInfo.SetVisible", value); }
		}
	}
}