using Lidgren.Network;

namespace SquareCubed.Client.Structures
{
	class StructuresNetwork
	{
		private readonly Structures _callback;

		public StructuresNetwork(Client client, Structures callback)
		{
			_callback = callback;
			client.Network.PacketHandlers.Bind("structures.data", OnStructureData);
		}

		private void OnStructureData(object s, NetIncomingMessage msg)
		{
			// Skip the packet type Id
			msg.ReadUInt16();
			msg.SkipPadBits();

			// Read the data
			var structure = msg.ReadStructure();

			// Pass the data on
			_callback.OnStructureData(structure);
		}
	}
}
