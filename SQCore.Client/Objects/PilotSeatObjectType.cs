using SQCore.Client.Gui;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	class PilotSeatObjectType : IObjectType
	{
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;

		public PilotSeatObjectType(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_client = client;
			_panel = panel;
		}

		public IClientObject CreateNew()
		{
			return new PilotSeatObject(_client, _panel);
		}
	}
}
