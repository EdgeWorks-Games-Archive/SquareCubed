using System;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using SquareCubed.Network;

namespace SquareCubed.Client.Structures.Objects
{
	public class ObjectNetwork
	{
		private readonly Network.Network _network;
		private readonly PacketType _packetType;

		internal ObjectNetwork(Network.Network network)
		{
			Contract.Requires<ArgumentNullException>(network != null);

			_network = network;
			_packetType = _network.PacketTypes["objects"];
		}

		public NetOutgoingMessage CreateMessageFor(IClientObject obj)
		{
			Contract.Requires<ArgumentNullException>(obj != null);

			var msg = _network.Peer.CreateMessage();
			msg.Write(_packetType);
			msg.Write(obj.Id);
			return msg;
		}
	}
}