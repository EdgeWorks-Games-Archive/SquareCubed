using System;
using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client.Graphics;
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

			_tpPanel = new TeleporterPanel(client.OldGui);
			_tpPanel.DialogClose += OnDialogClose;

			Texture = new Texture2D("./Graphics/Objects/Teleporter.png");

			_client.Input.TrackKey(Key.X);
		}

		public Texture2D Texture { get; private set; }

		public ClientObjectBase CreateNew(ClientStructure parent)
		{
			return new TeleporterObject(parent, this, _client, _panel);
		}

		public void Dispose()
		{
			Texture.Dispose();
		}

		private void OnDialogClose(object sender, EventArgs e)
		{
			_client.Player.LockInput = false;
		}

		public void OpenDialog()
		{
			_tpPanel.OpenDialog();
			_client.Player.LockInput = true;
		}
	}
}