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
			if (_peer != null && _peer.Status != NetPeerStatus.NotRunning)
				_peer.Shutdown("Network object disposed!");
		}

		#endregion

		#region Connection Management

		private NetPeer _peer;

		private void Start<T>(int port = -1) where T : NetPeer
		{
			// Log it
			_logger.LogInfo("Starting new {0}...", typeof (T).Name);

			// Make sure the peer is not already in use
			if (_peer != null) throw new Exception("Network peer already in use.");

			// Configure peer
			var config = new NetPeerConfiguration(_appIdentifier);
			if (port > 0) config.Port = port;

			// Start peer
			_peer = (T) Activator.CreateInstance(typeof (T), config);
			_peer.Start();
		}

		public void StartServer(int port = 12321)
		{
			// Configure and start server peer
			Start<NetServer>(port);
		}

		public void Connect(string host, int port = 12321)
		{
			// Configure and start client peer
			Start<NetClient>();

			// Attempt to connect to server
			Debug.Assert(_peer != null);
			_peer.Connect(host, port);
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
		public void BindPacketHandler(short type, EventHandler<NetIncomingMessage> e)
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
			if (_peer == null) return;

			NetIncomingMessage msg;
			while ((msg = _peer.ReadMessage()) != null)
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
						var type = msg.ReadInt16();
						if (_packetHandlers[type] != null)
							_packetHandlers[type].Invoke(this, msg);
						break;
					default:
						_logger.LogInfo("Unhandled message: " + msg.MessageType);
						break;
				}
				_peer.Recycle(msg);
			}
		}

		#endregion
	}
}