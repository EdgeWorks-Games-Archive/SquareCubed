using System;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using OpenTK;

namespace SquareCubed.Server.Structures.Objects
{
	public abstract class ServerObjectBase
	{
		private static int _idCounter;

		protected ServerObjectBase(IServerObjectType type)
		{
			Id = _idCounter++;
			Type = type;
		}

		public int Id { get; private set; }
		public IServerObjectType Type { get; private set; }
		public Vector2 Position { get; set; }
	}

	public abstract class NetworkServerObjectBase : ServerObjectBase
	{
		protected NetworkServerObjectBase(IServerObjectType type, ObjectsNetwork network) : base(type)
		{
			Contract.Requires<ArgumentNullException>(network != null);

			// TODO: Unregister on dispose
			network.Register(this);
		}

		public abstract void OnMessage(NetIncomingMessage msg);
	}
}