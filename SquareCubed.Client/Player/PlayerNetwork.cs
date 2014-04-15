using Lidgren.Network;
using SquareCubed.Network;

namespace SquareCubed.Client.Player
{
	internal class PlayerNetwork
	{
		private readonly Player _callback;
		private readonly Client _client;
		private readonly short _packetType;

		public PlayerNetwork(Client client, Player callback)
		{
			_client = client;
			_callback = callback;
			_packetType = _client.Network.PacketHandlers.ResolveType("players.data");
			_client.Network.PacketHandlers.Bind(_packetType, OnPlayerData);
		}

		private void OnPlayerData(object s, NetIncomingMessage msg)
		{
			// Skip the packet type Id
			msg.ReadUInt16();
			msg.SkipPadBits();

			// Read the data
			var key = msg.ReadUInt32();

			// Pass the data on
			_callback.OnPlayerData(key);
		}

		public void SendPlayerPhysics(PlayerUnit unit)
		{
			var msg = _client.Network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_packetType);
			msg.WritePadBits();

			// Add data
			msg.Write(unit.Position.X);
			msg.Write(unit.Position.Y);

			// Send data to server
			_client.Network.SendToServer(msg, NetDeliveryMethod.UnreliableSequenced, (int) SequenceChannels.UnitPhysics);
		}
	}
}