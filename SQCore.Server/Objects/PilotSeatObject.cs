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
		private float _throttle, _angularThrottle;
		private const float Speed = 4.0f, AngularSpeed = 8.0f;

		public PilotSeatObject(IServerObjectType type, SquareCubed.Server.Server server, ServerStructure parent)
			: base(type, server.Structures.ObjectsNetwork)
		{
			_parent = parent;
			server.UpdateTick += OnUpdateTick;
		}

		void OnUpdateTick(object sender, float delta)
		{
			var vec = new Vector2((float)Math.Sin(_parent.Rotation), (float)Math.Cos(_parent.Rotation)) * _throttle;
			_parent.Position = _parent.Position + vec * Speed * delta;
			_parent.Rotation = _parent.Rotation + _angularThrottle * AngularSpeed * delta;
		}

		public override void OnMessage(NetIncomingMessage msg)
		{
			_throttle = msg.ReadFloat();
			_angularThrottle = msg.ReadFloat();
		}
	}
}