using SquareCubed.Client.Player;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class PilotSeatObject : ClientObject
	{
		private readonly Player _player;
		private readonly UnitProximityHelper _proximity;

		public PilotSeatObject(SquareCubed.Client.Client client)
		{
			_player = client.Player;
			_proximity = new UnitProximityHelper(this);

			client.UpdateTick += Update;
		}

		private void Update(object s, float delta)
		{
			// Update the proximity helper, if there's no player it will default to not within range
			_proximity.Update(_player.PlayerUnit);

			//if (_proximity.Status == ProximityStatus.Within)
			// TODO: Display "Click on object to use!" hint here.
		}
	}
}