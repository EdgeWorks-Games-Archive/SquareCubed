using System;
using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	class PilotSeatObjectType : IServerObjectType
	{
		public ServerObjectBase CreateNew()
		{
			return new PilotSeatObject(this);
		}
	}
}
