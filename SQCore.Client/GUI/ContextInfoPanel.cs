using SquareCubed.Client.Gui;

namespace SQCore.Client.Gui
{
	public class ContextInfoPanel : GuiPanel
	{
		public ContextInfoPanel(SquareCubed.Client.Gui.Gui gui)
			: base(gui, "ContextInfo")
		{
		}

		public bool UseAltText
		{
			set { Gui.Trigger("contextinfo.usealt", value); }
		}

		public bool IsVisible
		{
			set { Gui.Trigger("contextinfo.visible", value); }
		}
	}
}