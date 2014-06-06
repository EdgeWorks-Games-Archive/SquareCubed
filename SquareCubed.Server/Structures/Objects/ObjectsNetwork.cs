using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lidgren.Network;

namespace SquareCubed.Server.Structures.Objects
{
	public class ObjectsNetwork
	{
		private readonly IDictionary<int, NetworkServerObjectBase> _objects = new Dictionary<int, NetworkServerObjectBase>();

		internal ObjectsNetwork(Network.Network network)
		{
			network.PacketHandlers.Bind(network.PacketTypes["objects"], OnObjectMessage);
		}

		public void Register(NetworkServerObjectBase obj)
		{
			Debug.Assert(obj != null);

			_objects.Add(obj.Id, obj);
		}

		private void OnObjectMessage(NetIncomingMessage msg)
		{
			// Try to get the object
			NetworkServerObjectBase obj;
			if (!_objects.TryGetValue(msg.ReadInt32(), out obj))
				return; // If it doesn't exist, it's probably an out of sync error

			// Pass over the message
			obj.OnMessage(msg);
		}
	}
}