using SquareCubed.Client.Gui;

namespace SQCore.Client.Gui
{
	public class ContextInfoPanel : GuiPanel
	{
		public ContextInfoPanel(SquareCubed.Client.Gui.Gui gui)
			: base(gui, "ContextInfo")
		{
		}

		public bool IsVisible
		{
			set { Gui.Trigger("ContextInfo.SetVisible", value); }
		}
	}
}