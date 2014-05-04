using Lidgren.Network;
using SquareCubed.Network;
using SquareCubed.Server.Players;

namespace SQCore.Server.Chat
{
	class ChatNetwork
	{
		private readonly Network _network;
		private readonly Chat _callback;
		private readonly PacketType _type;

		public ChatNetwork(Network network, Chat callback)
		{
			_network = network;
			_callback = callback;

			_type = _network.PacketTypes.RegisterType("chat");
			_network.PacketHandlers.Bind(_type, OnChatMessage);
		}

		public void SendWorldChatMessage(Player player, string message)
		{
			var msg = _network.Peer.CreateMessage();
			msg.Write(_type);
			msg.Write(player.Name);
			msg.Write(message);
			player.Unit.World.SendToAllPlayers(msg, NetDeliveryMethod.ReliableOrdered, (int)SequenceChannels.Chat);
		}

		private void OnChatMessage(NetIncomingMessage msg)
		{
			_callback.OnChatMessage(msg.SenderConnection, msg.ReadString());
		}
	}
}
