using Lidgren.Network;
using OpenTK;
using SquareCubed.Common.Data;
using SquareCubed.Network;

namespace SquareCubed.Server.Players
{
	internal class PlayersNetwork
	{
		private readonly Players _callback;
		private readonly Network.Network _network;
		private readonly PacketType _packetType;

		public PlayersNetwork(Network.Network network, Players callback)
		{
			_network = network;
			_callback = callback;

			_packetType = _network.PacketTypes.ResolveType("players.data");
			_network.PacketHandlers.Bind(_packetType, OnPlayerPhysics);
		}

		private void OnPlayerPhysics(NetIncomingMessage msg)
		{
			// Read the data
			var position = msg.ReadVector2();

			// Pass the data on
			_callback.OnPlayerPhysics(msg.SenderConnection, position);
		}

		public void SendPlayerData(Player player)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_packetType);

			// Send over unit Id so client can link it to the player
			msg.Write(player.Unit.Id);

			player.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.UnitData);
		}
	}
}