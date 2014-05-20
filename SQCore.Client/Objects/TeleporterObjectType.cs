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

		public TeleporterObjectType(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_client = client;
			_panel = panel;

			_client.Input.TrackKey(Key.X);
		}

		public ClientObjectBase CreateNew(ClientStructure parent)
		{
			return new TeleporterObject(_client, _panel, parent);
		}
	}
}