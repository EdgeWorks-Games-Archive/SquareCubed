using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Lidgren.Network;
using SquareCubed.Common.Utils;
using SquareCubed.Network;
using SquareCubed.PluginLoader;

namespace SquareCubed.Server.Meta
{
	public class Meta
	{
		private readonly Logger _logger = new Logger("Meta");
		private readonly PacketType _packetType;
		private readonly PluginLoader<IServerPlugin, Server> _pluginLoader;

		public event EventHandler<NetConnection> ClientDataReceived;

		public Meta(Network.Network network, PluginLoader<IServerPlugin, Server> pluginLoader)
		{
			Contract.Requires<ArgumentNullException>(network != null);
			Contract.Requires<ArgumentNullException>(pluginLoader != null);

			_pluginLoader = pluginLoader;

			network.NewConnection += OnNewConnection;

			// Resolve packet type num and bind handler
			_packetType = network.PacketTypes.ResolveType("meta");
			network.PacketHandlers.Bind(_packetType, OnMetaPacket);
		}

		private void OnNewConnection(object sender, NetIncomingMessage msg)
		{
			_logger.LogInfo("Sending MetaData to {0:X}...", msg.SenderConnection.RemoteUniqueIdentifier);

			// Start building the meta handshake message
			var outMsg = msg.SenderConnection.Peer.CreateMessage();

			// Write packet type
			outMsg.Write(_packetType);

			// Write packet type mapping data

			// Write mod data
			outMsg.Write(_pluginLoader.PluginTypes.Count);
			foreach (var type in _pluginLoader.PluginTypes)
			{
				// Write plugin type data
				outMsg.Write(type.Key);
				// Server is assumed to only have one version of each plugin
				outMsg.Write(type.Value.Versions.First().Key.ToString());
			}

			// Send out the message
			msg.SenderConnection.SendMessage(outMsg, NetDeliveryMethod.ReliableUnordered, 0);
		}

		private void OnMetaPacket(NetIncomingMessage msg)
		{
			ClientDataReceived(this, msg.SenderConnection);
		}
	}
}