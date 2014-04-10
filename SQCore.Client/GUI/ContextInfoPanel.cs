using SquareCubed.Client.Gui;

namespace SQCore.Client.GUI
{
	public class ContextInfoPanel : GuiPanel
	{
		private readonly Gui _gui;

		public ContextInfoPanel(Gui gui)
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