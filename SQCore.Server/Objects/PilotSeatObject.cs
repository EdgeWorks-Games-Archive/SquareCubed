using System;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	internal sealed class PilotSeatObject : NetworkServerObjectBase
	{
		private readonly ServerStructure _parent;
		private float _throttle;

		public PilotSeatObject(PilotSeatObjectType type, SquareCubed.Server.Server server, ServerStructure parent)
			: base(type, server.Structures.ObjectsNetwork)
		{
			_parent = parent;
			server.UpdateTick += OnUpdateTick;
		}

		void OnUpdateTick(object sender, float e)
		{
			var vec = new Vector2((float)Math.Sin(_parent.Rotation), (float)Math.Cos(_parent.Rotation));
			vec *= _throttle;
			_parent.Position = _parent.Position + vec;
		}

		public override void OnMessage(NetIncomingMessage msg)
		{
			var throttle = msg.ReadFloat();
			_throttle = throttle;
		}
	}
}