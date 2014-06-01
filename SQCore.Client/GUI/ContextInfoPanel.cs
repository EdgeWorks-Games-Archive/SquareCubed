using SquareCubed.Client.Gui;

namespace SQCore.Client.Gui
{
	public class ContextInfoPanel : GuiPanel
	{
		private int _visible;

		public ContextInfoPanel(SquareCubed.Client.Gui.OldGui oldGui)
			: base(oldGui, "ContextInfo")
		{
		}

		public string Text
		{
			set { OldGui.Trigger("contextinfo.text", value); }
		}

		public int VisibleCount
		{
			get { return _visible; }
			set
			{
				if (_visible == 0 && value != 0)
					OldGui.Trigger("contextinfo.visible", true);
				else if (_visible != 0 && value == 0)
					OldGui.Trigger("contextinfo.visible", false);

				_visible = value;
			}
		}
	}
}