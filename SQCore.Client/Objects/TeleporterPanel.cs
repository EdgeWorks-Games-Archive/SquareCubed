using System;
using SquareCubed.Client.Gui;

namespace SQCore.Client.Objects
{
	internal class TeleporterPanel : GuiPanel
	{
		public event EventHandler DialogClose = (o, e) => { };

		public TeleporterPanel(SquareCubed.Client.Gui.Gui gui)
			: base(gui, "Teleporter")
		{
			gui.BindCall("teleporter.onclose", OnClose);
		}

		private void OnClose()
		{
			DialogClose(this, EventArgs.Empty);
		}

		public void OpenDialog()
		{
			Gui.Trigger("teleporter.show");
		}
	}
}