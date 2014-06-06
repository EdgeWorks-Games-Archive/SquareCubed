using System;
using System.Diagnostics;
using System.Linq;
using Lidgren.Network;
using SquareCubed.Common.Utils;
using SquareCubed.Network;
using SquareCubed.PluginLoader;

namespace SquareCubed.Server.Meta
{
	public class Meta
	{
		private readonly Network.Network _network;
		private readonly Logger _logger = new Logger("Meta");
		private readonly PacketType _packetType;
		private readonly PluginLoader<IServerPlugin, Server> _pluginLoader;

		public event EventHandler<NetConnection> ClientDataReceived;

		public Meta(Network.Network network, PluginLoader<IServerPlugin, Server> pluginLoader)
		{
			Debug.Assert(network != null);
			Debug.Assert(pluginLoader != null);

			_network = network;
			_pluginLoader = pluginLoader;

			network.NewConnection += OnNewConnection;

			// Resolve packet type num and bind handler
			_packetType = _network.PacketTypes.ResolveType("meta");
			_network.PacketHandlers.Bind(_packetType, OnMetaPacket);
		}

		private void OnNewConnection(object sender, NetIncomingMessage msg)
		{
			_logger.LogInfo("Sending MetaData to {0:X}...", msg.SenderConnection.RemoteUniqueIdentifier);

			// Start building the meta handshake message
			var outMsg = msg.SenderConnection.Peer.CreateMessage();

			// Write packet type
			outMsg.Write(_packetType);

			// Write packet type mapping data
			var types = _network.PacketTypes.Types;
			outMsg.Write(types.Count);
			foreach (var type in types)
			{
				outMsg.Write(type.Key);
				outMsg.Write(type.Value.Id);
			}

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