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
		private const float Speed = 1.0f, AngularSpeed = 1.6f;

		public PilotSeatObject(IServerObjectType type, SquareCubed.Server.Server server, ServerStructure parent)
			: base(type, server.Structures.ObjectsNetwork)
		{
			_parent = parent;
			server.UpdateTick += OnUpdateTick;
		}

		void OnUpdateTick(object sender, float delta)
		{
			_parent.Torque = _angularThrottle * AngularSpeed;
			var force = new Vector2((float)Math.Sin(_parent.Body.Rotation), (float)Math.Cos(_parent.Body.Rotation)) * _throttle * Speed;
			_parent.Force = force;
		}

		public override void OnMessage(NetIncomingMessage msg)
		{
			_throttle = msg.ReadFloat();
			_angularThrottle = msg.ReadFloat();
		}
	}
}