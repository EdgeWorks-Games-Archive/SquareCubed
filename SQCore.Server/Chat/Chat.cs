using SquareCubed.Network;

namespace SQCore.Server.Chat
{
	class Chat
	{
		public Chat(Network network)
		{
			network.PacketTypes.RegisterType("chat");
		}
	}
}
