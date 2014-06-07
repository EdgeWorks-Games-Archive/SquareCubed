using SquareCubed.Server.Structures;
using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	sealed class PilotSeatObjectType : IServerObjectType
	{
		private readonly SquareCubed.Server.Server _server;

		public PilotSeatObjectType(SquareCubed.Server.Server server)
		{
			_server = server;
		}

		public ServerObjectBase CreateNew(ServerStructure parent)
		{
			return new PilotSeatObject(this, _server, parent);
		}
	}
}
