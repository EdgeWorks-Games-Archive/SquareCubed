using System;
using System.Diagnostics;
using Lidgren.Network;
using SquareCubed.Network;

namespace SquareCubed.Client.Structures.Objects
{
	public class ObjectsNetwork
	{
		private readonly Network.Network _network;
		private readonly PacketType _packetType;

		internal ObjectsNetwork(Network.Network network)
		{
			Debug.Assert(network != null);

			_network = network;
			_packetType = _network.PacketTypes["objects"];
		}

		public NetOutgoingMessage CreateMessageFor(ClientObjectBase obj)
		{
			Debug.Assert(obj != null);

			var msg = _network.Peer.CreateMessage();
			msg.Write(_packetType);
			msg.Write(obj.Id);
			return msg;
		}
	}
}