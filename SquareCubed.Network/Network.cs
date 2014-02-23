using System;
using System.Diagnostics;
using Lidgren.Network;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Network
{
	public class Network : IDisposable
	{
		private readonly Logger _logger = new Logger("Network");

		#region Network Properties

		private readonly string _appIdentifier;
		public NetPeer Peer { get; private set; }
		public PacketHandlers PacketHandlers { get; set; }

		#region Client/Server Specific Data

		private bool _isServer;
		public NetConnection Server { get; set; }

		#endregion

		#endregion

		#region Initialization and Cleanup

		private bool _disposed;

		public Network(string appIdentifier)
		{
			_appIdentifier = appIdentifier;
			PacketHandlers = new PacketHandlers();

			// Register common packet Ids that never change.
			// Usually you would only do this on the server
			// side of a mod, but these are used by core parts
			// of the engine.
			PacketHandlers.RegisterTypeId("meta", 0);
			PacketHandlers.RegisterTypeId("units.physics", 1);
			PacketHandlers.RegisterTypeId("units.data", 2);
			PacketHandlers.RegisterTypeId("players.data", 3);
		}

		public virtual void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Prevent double disposing and don't dispose if we're told not to
			if (_disposed || !disposing) return;
			_disposed = true;

			// Dispose stuff here
			if (Peer != null && Peer.Status != NetPeerStatus.NotRunning)
				Peer.Shutdown("Network object disposed!");
		}

		#endregion

		#region Connection Management

		private void Start<T>(int port = -1) where T : NetPeer
		{
			// Log it
			_logger.LogInfo("Starting new {0}...", typeof (T).Name);

			// Make sure the peer is not already in use
			if (Peer != null) throw new Exception("Network peer already in use.");

			// Configure peer
			var config = new NetPeerConfiguration(_appIdentifier);
			if (port > 0) config.Port = port;

			// Start peer
			Peer = (T) Activator.CreateInstance(typeof (T), config);
			Peer.Start();
		}

		public void StartServer(int port = 12321)
		{
			// Configure and start server peer
			Start<NetServer>(port);
			_isServer = true;
		}

		public void Connect(string host, int port = 12321)
		{
			// Configure and start client peer
			Start<NetClient>();
			_isServer = false;

			// Attempt to connect to server
			Debug.Assert(Peer != null);
			Peer.Connect(host, port);
		}

		#endregion

		#region Packet Handling

		public event EventHandler<NetIncomingMessage> NewConnection;

		public void HandlePackets()
		{
			// If not connected, do nothing
			if (Peer == null) return;

			NetIncomingMessage msg;
			while ((msg = Peer.ReadMessage()) != null)
			{
				switch (msg.MessageType)
				{
					case NetIncomingMessageType.VerboseDebugMessage:
					case NetIncomingMessageType.DebugMessage:
						break; // We don't need those
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.ErrorMessage:
						_logger.LogInfo(msg.ReadString());
						break;
					case NetIncomingMessageType.StatusChanged:
						var status = (NetConnectionStatus) msg.ReadByte();
						_logger.LogInfo("Status changed to {0}: {1}", status.ToString(), msg.ReadString());

						// Fire new connection event
						if (status == NetConnectionStatus.Connected)
						{
							if (!_isServer) Server = msg.SenderConnection;
							if (NewConnection != null) NewConnection(this, msg);
						}

						break;
					case NetIncomingMessageType.Data:
						PacketHandlers.HandlePacket(msg, _isServer);
						break;
					default:
						_logger.LogInfo("Unhandled message: " + msg.MessageType);
						break;
				}
				Peer.Recycle(msg);
			}
		}

		#endregion

		#region Packet Sending

		public void SendToServer(NetOutgoingMessage msg, NetDeliveryMethod method, int sequenceChannel = -1)
		{
			if (_isServer) throw new Exception("Can't send to server, we are the server!");

			Peer.SendMessage(
				msg,
				Server,
				method,
				sequenceChannel);
		}

		#endregion
	}
}