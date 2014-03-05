using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Lidgren.Network;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Server.Meta
{
	public class Meta
	{
		private readonly Logger _logger = new Logger("Meta");
		private readonly ushort _packetType;
		private readonly Server _server;

		public event EventHandler<NetConnection> ClientDataReceived;

		public Meta(Server server)
		{
			Contract.Requires(server != null);

			_server = server;
			_server.Network.NewConnection += OnNewConnection;

			// Resolve packet type num and bind handler
			_packetType = _server.Network.PacketHandlers.ResolveType("meta");
			_server.Network.PacketHandlers.Bind(_packetType, OnMetaPacket);
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
			outMsg.Write((ushort) _server.PluginLoader.PluginTypes.Count);
			outMsg.WritePadBits();
			foreach (var type in _server.PluginLoader.PluginTypes)
			{
				// Write plugin type data
				outMsg.Write(type.Key);
				// Server is assumed to only have one version of each plugin
				outMsg.Write(type.Value.Versions.First().Key.ToString());
			}

			// Send out the message
			msg.SenderConnection.SendMessage(outMsg, NetDeliveryMethod.ReliableUnordered, 0);
		}

		private void OnMetaPacket(object sender, NetIncomingMessage msg)
		{
			ClientDataReceived(this, msg.SenderConnection);
		}
	}
}