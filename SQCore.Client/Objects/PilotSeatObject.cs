using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	class PilotSeatObject : ClientObject
	{
		private UnitProximityHelper _proximity;

		public PilotSeatObject(SquareCubed.Client.Client client)
		{
			_proximity = new UnitProximityHelper(this);
		}
	}
}