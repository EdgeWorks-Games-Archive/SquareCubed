using Lidgren.Network;

namespace SquareCubed.Server.Structures.Objects
{
	public class ObjectNetwork
	{
		private readonly Network.Network _network;

		internal ObjectNetwork(Network.Network network)
		{
			_network = network;
			_network.PacketHandlers.Bind(_network.PacketTypes["objects"], OnObjectPacket);
		}

		private void OnObjectPacket(NetIncomingMessage msg)
		{
		}
	}
}
