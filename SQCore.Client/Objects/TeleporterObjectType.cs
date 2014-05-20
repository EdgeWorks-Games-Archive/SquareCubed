using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal sealed class TeleporterObjectType : IClientObjectType
	{
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;
		private readonly TeleporterPanel _tpPanel;

		public TeleporterObjectType(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_client = client;
			_panel = panel;
			_tpPanel = new TeleporterPanel(client.Gui);
			_tpPanel.DialogClose += OnDialogClose;

			_client.Input.TrackKey(Key.X);
		}

		private void OnDialogClose(object sender, System.EventArgs e)
		{
			_client.Player.LockInput = false;
		}

		public void OpenDialog()
		{
			_tpPanel.OpenDialog();
			_client.Player.LockInput = true;
		}

		public ClientObjectBase CreateNew(ClientStructure parent)
		{
			return new TeleporterObject(parent, this, _client, _panel);
		}
	}
}