using SquareCubed.Server.Structures;
using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	sealed class TeleporterObjectType : IServerObjectType
	{
		public ServerObjectBase CreateNew(ServerStructure parent)
		{
			return new TeleporterObject(this);
		}
	}
}
