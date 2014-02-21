using System;
using Lidgren.Network;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Client
{
	class Meta
	{
		private readonly Logger _logger = new Logger("Meta");
		private Client _client;

		public Meta(Client client)
		{
			_client = client;
			_client.Network.BindPacketHandler(0, OnMetaPacket);
		}

		private void OnMetaPacket(object sender, NetIncomingMessage msg)
		{
			_logger.LogInfo("Received MetaData!");

			// Read packet type mapping data

			// Read mod data
			var count = msg.ReadInt16();
			msg.ReadPadBits();
			for (var i = 0; i < count; i++)
			{
				var id = msg.ReadString();
				var version = new Version(msg.ReadString());
				_client.PluginLoader.LoadPlugin(id, version, _client);
			}
		}
	}
}
