using Lidgren.Network;

namespace SquareCubed.Client.Structures
{
	internal class StructuresNetwork
	{
		private readonly Structures _callback;

		public StructuresNetwork(Network.Network network, Structures callback)
		{
			_callback = callback;
			network.PacketHandlers.Bind("structures.physics", OnStructurePhysics);
			network.PacketHandlers.Bind("structures.data", OnStructureData);
		}

		private void OnStructurePhysics(NetIncomingMessage msg)
		{
			// TODO: Add structure physics sync here.
		}

		private void OnStructureData(NetIncomingMessage msg)
		{
			// Read the data
			var structure = msg.ReadStructure(_callback.ObjectTypes);

			// Pass the data on
			_callback.OnStructureData(structure);
		}
	}
}