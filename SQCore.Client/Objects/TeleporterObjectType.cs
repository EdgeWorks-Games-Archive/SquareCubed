using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	sealed class TeleporterObjectType : IClientObjectType
	{
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;

		public TeleporterObjectType(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_client = client;
			_panel = panel;

			_client.Input.TrackKey(Key.X);
		}

		public IClientObject CreateNew()
		{
			return new TeleporterObject(_client, _panel);
		}
	}
}
