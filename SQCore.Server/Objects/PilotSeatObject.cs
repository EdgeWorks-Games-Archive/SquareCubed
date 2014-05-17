using Lidgren.Network;
using OpenTK;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	internal sealed class PilotSeatObject : NetworkServerObjectBase
	{
		private readonly ServerStructure _parent;

		public PilotSeatObject(PilotSeatObjectType type, SquareCubed.Server.Server server, ServerStructure parent)
			: base(type, server.Structures.ObjectsNetwork)
		{
			_parent = parent;
			server.UpdateTick += server_UpdateTick;
		}

		void server_UpdateTick(object sender, float e)
		{
			_parent.Position = _parent.Position + new Vector2(0, 0.1f);
		}

		public override void OnMessage(NetIncomingMessage msg)
		{
			
		}
	}
}