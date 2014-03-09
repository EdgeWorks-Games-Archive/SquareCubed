using System;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Meta
{
	public class Meta
	{
		private readonly Logger _logger = new Logger("Meta");
		private readonly ushort _packetType;
		private readonly Client _client;

		public Meta(Client client)
		{
			Contract.Requires<ArgumentNullException>(client != null);
			_client = client;

			// Resolve packet type num and bind handler
			_packetType = _client.Network.PacketHandlers.ResolveType("meta");
			_client.Network.PacketHandlers.Bind(_packetType, OnMetaPacket);
		}

		private void OnMetaPacket(object sender, NetIncomingMessage msg)
		{
			if(_client.PluginLoader.LoadedPlugins.Count != 0)
				throw new Exception("MetaData received but mods already loaded!");

			_logger.LogInfo("Received MetaData!");

			// Skip Initial Type
			msg.ReadUInt16();

			// Read packet type mapping data

			// Read tile type mapping data
			
			// Read wall type mapping data

			// Read mod data
			var count = msg.ReadUInt16();
			msg.SkipPadBits();
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
