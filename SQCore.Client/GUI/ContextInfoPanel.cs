using SquareCubed.Client.Gui;

namespace SQCore.Client.Gui
{
	public class ContextInfoPanel : GuiPanel
	{
		private int _visible;

		public ContextInfoPanel(SquareCubed.Client.Gui.Gui gui)
			: base(gui, "ContextInfo")
		{
		}

		public string Text
		{
			set { Gui.Trigger("contextinfo.text", value); }
		}

		public int VisibleCount
		{
			get { return _visible; }
			set
			{
				if (_visible == 0 && value != 0)
					Gui.Trigger("contextinfo.visible", true);
				else if (_visible != 0 && value == 0)
					Gui.Trigger("contextinfo.visible", false);

				_visible = value;
			}
		}
	}
}