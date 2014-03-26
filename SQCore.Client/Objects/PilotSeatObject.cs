using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	class PilotSeatObject : ClientObject
	{
		private PlayerProximityHelper _proximityHelper;

		public PilotSeatObject(SquareCubed.Client.Client client)
		{
			_proximityHelper = new PlayerProximityHelper(this, client.Player);
		}
	}
}
