using Lidgren.Network;
using SquareCubed.Network;

namespace SquareCubed.Client.Player
{
	internal class PlayerNetwork
	{
		private readonly Player _callback;
		private readonly Network.Network _network;
		private readonly PacketType _packetType;

		public PlayerNetwork(Network.Network network, Player callback)
		{
			_network = network;
			_callback = callback;

			_packetType = _network.PacketTypes.ResolveType("players.data");
			_network.PacketHandlers.Bind(_packetType, OnPlayerData);
		}

		private void OnPlayerData(NetIncomingMessage msg)
		{
			// Read the data
			var key = msg.ReadInt32();

			// Pass the data on
			_callback.OnPlayerData(key);
		}

		public void SendPlayerPhysics(PlayerUnit unit)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_packetType);

			// Add data
			msg.Write(unit.Position.X);
			msg.Write(unit.Position.Y);

			// Send data to server
			_network.SendToServer(msg, NetDeliveryMethod.UnreliableSequenced, (int)SequenceChannels.UnitPhysics);
		}
	}
}