using Lidgren.Network;
using SquareCubed.Network;

namespace SQCore.Client.Chat
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

			_type = network.PacketTypes.ResolveType("chat");
			network.PacketHandlers.Bind(_type, OnChatMessage);
		}

		public void SendChatMessage(string message)
		{
			var msg = _network.Peer.CreateMessage();
			msg.Write(_type);
			msg.Write(message);
			_network.Server.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, (int)SequenceChannels.Chat);
		}

		private void OnChatMessage(NetIncomingMessage msg)
		{
			_callback.OnChatMessage(msg.ReadString(), msg.ReadString());
		}
	}
}
