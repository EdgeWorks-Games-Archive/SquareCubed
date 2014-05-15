using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	class PilotSeatObjectType : IClientObjectType
	{
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;

		public PilotSeatObjectType(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_client = client;
			_panel = panel;

			_client.Input.TrackKey(Key.ShiftLeft);
			_client.Input.TrackKey(Key.ControlLeft);
			_client.Input.TrackKey(Key.X);
		}

		public IClientObject CreateNew()
		{
			return new PilotSeatObject(_client, _panel);
		}
	}
}
