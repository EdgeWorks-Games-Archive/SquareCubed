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

		private bool _isServer;

		#endregion

		#region Initialization and Cleanup

		private bool _disposed;

		public Network(string appIdentifier)
		{
			_appIdentifier = appIdentifier;
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

		#region Events

		public event EventHandler<NetIncomingMessage> NewConnection;
		private readonly PacketType[] _packetHandlers = new PacketType[10];

		private class PacketType
		{
			public event EventHandler<NetIncomingMessage> Handler;

			public void Invoke(object sender, NetIncomingMessage msg)
			{
				Handler(sender, msg);
			}
		}

		// TODO: Change to use key instead of number
		public void BindPacketHandler(ushort type, EventHandler<NetIncomingMessage> e)
		{
			if (_packetHandlers[type] == null)
			{
				_packetHandlers[type] = new PacketType();
				_packetHandlers[type].Handler += e;
			}
			else
				throw new Exception("Packet handler already bound to this type!");
		}

		#endregion

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
							var newConnection = NewConnection;
							if (newConnection != null) NewConnection(this, msg);
						}

						break;
					case NetIncomingMessageType.Data:
						try
						{
							// Read the packet type
							var type = msg.ReadUInt16();

							// If we have a packet handler on this type, invoke it, if not throw to drop client
							if (_packetHandlers[type] != null)
								_packetHandlers[type].Invoke(this, msg);
							else
							{
								if (_isServer)
									throw new Exception(string.Format("Connection sent invalid packet type {0}!", type));
								_logger.LogInfo("Connection sent invalid packet type {0}!", type);
							}
						}
						catch (Exception e)
						{
							// Assume client is doing something wrong, drop them to be sure
							// Be very careful if you're changing this behavior, an exception in the packet handling
							// means there's something wrong with the packet the client is sending or the server's code
							// that's handling the packet. Exceptions are slow to process and if you don't drop the client
							// this could be a DoS vulnerability. Sure it might be a server issue but it's less of a
							// problem to drop a legit client then it is to have a server go down.
							_logger.LogInfo("Exception occurred during packet handling: " + e.Message);
							_logger.LogInfo("Dropping client {0}!", msg.SenderConnection.RemoteUniqueIdentifier);
							msg.SenderConnection.Disconnect(e.Message);
						}
						break;
					default:
						_logger.LogInfo("Unhandled message: " + msg.MessageType);
						break;
				}
				Peer.Recycle(msg);
			}
		}

		#endregion
	}
}