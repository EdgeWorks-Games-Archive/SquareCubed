using System;
using SquareCubed.Client.Gui;

namespace SQCore.Client.Objects
{
	internal class TeleporterPanel : GuiPanel
	{
		public event EventHandler DialogClose = (o, e) => { };

		public TeleporterPanel(SquareCubed.Client.Gui.OldGui oldGui)
			: base(oldGui, "Teleporter")
		{
			oldGui.BindCall("teleporter.onclose", OnClose);
		}

		private void OnClose()
		{
			DialogClose(this, EventArgs.Empty);
		}

		public void OpenDialog()
		{
			OldGui.Trigger("teleporter.show");
		}
	}
}