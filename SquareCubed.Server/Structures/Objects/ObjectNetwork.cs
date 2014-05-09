using Lidgren.Network;

namespace SquareCubed.Server.Structures.Objects
{
	public class ObjectNetwork
	{
		internal ObjectNetwork(Network.Network network)
		{
			network.PacketHandlers.Bind(network.PacketTypes["objects"], OnObjectPacket);
		}

		private void OnObjectPacket(NetIncomingMessage msg)
		{
		}
	}
}
