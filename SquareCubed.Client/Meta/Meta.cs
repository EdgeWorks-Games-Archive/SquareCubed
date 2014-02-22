using System;
using Lidgren.Network;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Client.Meta
{
	public class Meta
	{
		private readonly Logger _logger = new Logger("Meta");
		private readonly ushort _packetType;
		private readonly Client _client;

		public Meta(Client client)
		{
			_client = client;

			// TODO: Change to read packet type from Network module
			_packetType = 0;
			_client.Network.BindPacketHandler(_packetType, OnMetaPacket);
		}

		private void OnMetaPacket(object sender, NetIncomingMessage msg)
		{
			if(_client.PluginLoader.LoadedPlugins.Count != 0)
				throw new Exception("MetaData received but mods already loaded!");

			_logger.LogInfo("Received MetaData!");

			// Read packet type mapping data

			// Read mod data
			var count = msg.ReadUInt16();
			msg.ReadPadBits(); // TODO: replace with skipadbits? right now trying to fix bug
			for (var i = 0; i < count; i++)
			{
				var id = msg.ReadString();
				var version = new Version(msg.ReadString());
				_client.PluginLoader.LoadPlugin(id, version, _client);
			}

			// Send a message that MetaData has been processed
			var outMsg = _client.Network.Peer.CreateMessage();
			outMsg.Write(_packetType);
			msg.SenderConnection.SendMessage(outMsg, NetDeliveryMethod.ReliableUnordered, 0);
		}
	}
}
