using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	sealed class TeleporterObjectType : IClientObjectType
	{
		public IClientObject CreateNew()
		{
			return new TeleporterObject(this);
		}
	}
}
