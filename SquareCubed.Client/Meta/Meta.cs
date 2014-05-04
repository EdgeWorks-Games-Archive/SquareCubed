using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Lidgren.Network;
using SquareCubed.Common.Utils;
using SquareCubed.Network;

namespace SquareCubed.Client.Meta
{
	public class Meta
	{
		private readonly Logger _logger = new Logger("Meta");
		private readonly PacketType _packetType;
		private readonly Client _client;

		public Meta(Client client)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			_client = client;

			// Resolve packet type num and bind handler
			_packetType = _client.Network.PacketTypes.ResolveType("meta");
			_client.Network.PacketHandlers.Bind(_packetType, OnMetaPacket);
		}

		private void OnMetaPacket(NetIncomingMessage msg)
		{
			if(_client.PluginLoader.LoadedPlugins.Count != 0)
				throw new Exception("MetaData received but mods already loaded!");

			_logger.LogInfo("Received MetaData!");

			// Read packet type mapping data
			var count = msg.ReadInt32();
			for (var i = 0; i < count; i++)
			{
				var name = msg.ReadString();
				var id = msg.ReadInt32();

				// If all entires don't match this, add a new one
				if(_client.Network.PacketTypes.Types.All(t => t.Key != name))
					_client.Network.PacketTypes.RegisterType(name, id);
			}

			// Read tile type mapping data
			
			// Read wall type mapping data

			// Read mod data
			count = msg.ReadInt32();
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
