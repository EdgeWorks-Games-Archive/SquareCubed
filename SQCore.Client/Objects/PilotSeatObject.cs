using SquareCubed.Client;
using SquareCubed.Client.Gui;
using SquareCubed.Client.Player;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class PilotSeatObject : ClientObject
	{
		private readonly Player _player;
		private readonly Gui _gui;
		private readonly UnitProximityHelper _proximity;
		
		public PilotSeatObject(SquareCubed.Client.Client client)
		{
			_gui = client.Gui;
			_player = client.Player;
			_proximity = new UnitProximityHelper(this, 1.0f);

			client.UpdateTick += Update;
		}

		private void Update(object s, TickEventArgs e)
		{
			// Update the proximity helper, if there's no player it will default to not within range
			_proximity.Update(_player.PlayerUnit);

			// Actually do something with this data
			// TODO: Change to use a status change event in UnitProximityHelper instead
			_gui.Trigger("SetContextInfoVisibility", _proximity.Status == ProximityStatus.Within);
		}
	}
}