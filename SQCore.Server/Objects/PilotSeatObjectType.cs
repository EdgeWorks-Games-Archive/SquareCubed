using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	class PilotSeatObjectType : IServerObjectType
	{
		private readonly ObjectsNetwork _network;

		public PilotSeatObjectType(ObjectsNetwork network)
		{
			_network = network;
		}

		public ServerObjectBase CreateNew()
		{
			return new PilotSeatObject(this, _network);
		}
	}
}
