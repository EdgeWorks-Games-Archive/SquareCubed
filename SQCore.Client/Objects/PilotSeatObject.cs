using SquareCubed.Client;
using SquareCubed.Client.Gui;
using SquareCubed.Client.Player;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class PilotSeatObject : ClientObject
	{
		private readonly Gui _gui;
		private readonly Player _player;
		private readonly UnitProximityHelper _proximity;

		public PilotSeatObject(SquareCubed.Client.Client client)
		{
			_gui = client.Gui;
			_player = client.Player;
			_proximity = new UnitProximityHelper(this);

			client.UpdateTick += Update;
			_proximity.Change += OnChange;
		}

		private void Update(object s, TickEventArgs e)
		{
			// Update the proximity helper, if there's no player it will default to not within range
			_proximity.Update(_player.PlayerUnit);
		}

		private void OnChange(object s, ProximityEventArgs e)
		{
			// Actually do something with this data
			_gui.Trigger("SetContextInfoVisibility", e.NewStatus == ProximityStatus.Within);
		}
	}
}