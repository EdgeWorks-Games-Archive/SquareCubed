using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	class PilotSeatObject : NetworkServerObjectBase
	{
		public PilotSeatObject(PilotSeatObjectType type, ObjectsNetwork network)
			: base(type, network)
		{
		}
	}
}
