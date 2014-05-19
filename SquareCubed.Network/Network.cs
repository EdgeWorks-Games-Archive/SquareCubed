using System;
using System.Diagnostics;
using Lidgren.Network;
using SquareCubed.Common.Utils;

namespace SquareCubed.Network
{
	public sealed class Network : IDisposable
	{
		private readonly Logger _logger = new Logger("Network");

		#region Network Properties

		private readonly string _appIdentifier;
		public NetPeer Peer { get; private set; }
		public PacketHandlers PacketHandlers { get; private set; }
		public PacketTypes PacketTypes { get; private set; }

		#region Client/Server Specific Data

		private bool _isServer;
		public NetConnection Server { get; set; }

		#endregion

		#endregion

		#region Initialization and Cleanup

		public Network(string appIdentifier)
		{
			_appIdentifier = appIdentifier;
			PacketTypes = new PacketTypes();
			PacketHandlers = new PacketHandlers();

			// Register common packet Ids that never change.
			// Usually you would only do this on the server
			// side of a mod and let the engine decide what
			// numeric value to use, but these are used by
			// core parts of the engine.
			PacketTypes.RegisterType("meta");
			PacketTypes.RegisterType("units.physics");
			PacketTypes.RegisterType("units.data");
			PacketTypes.RegisterType("units.teleport");
			PacketTypes.RegisterType("players.data");
			PacketTypes.RegisterType("structures.physics");
			PacketTypes.RegisterType("structures.data");
			PacketTypes.RegisterType("objects");
		}

		public void Dispose()
		{
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
			if (_isServer) config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

			// Start peer
			Peer = (T) Activator.CreateInstance(typeof (T), config);
			Peer.Start();
		}

		public void StartServer(int port = 12321)
		{
			// Configure and start server peer
			_isServer = true;
			Start<NetServer>(port);
		}

		public void Connect(string host, string playerName, int port = 12321)
		{
			// Configure and start client peer
			_isServer = false;
			Start<NetClient>();

			// Attempt to connect to server
			Debug.Assert(Peer != null);
			var name = Peer.CreateMessage();
			name.Write(playerName);
			Peer.Connect(host, port, name);
		}

		public void Disconnect()
		{
			Server.Disconnect("Disconnect called!");
		}

		#endregion

		#region Packet Handling

		public event EventHandler<NetIncomingMessage> NewConnection = (s, e) => { };
		public event EventHandler<NetIncomingMessage> LostConnection = (s, e) => { };
		public event EventHandler<NetIncomingMessage> FailedConnection = (s, e) => { };

		/// <summary>
		/// To react to this event, set the Deny in the event arguments
		/// to true if you need to deny an incoming connection.
		/// </summary>
		public event EventHandler<ConnectApprovalEventArgs> ApprovalRequested = (s, e) => { };

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

					case NetIncomingMessageType.ConnectionApproval:
						// Check if anyone wants to deny this connect request
						var arguments = new ConnectApprovalEventArgs(msg.ReadString(), msg.SenderConnection);
						ApprovalRequested(this, arguments);
						if (arguments.Deny)
							msg.SenderConnection.Deny();
						else
							msg.SenderConnection.Approve();

						break;

					case NetIncomingMessageType.StatusChanged:
						var status = (NetConnectionStatus) msg.ReadByte();
						_logger.LogInfo("Status changed to {0}: {1}", status.ToString(), msg.ReadString());

						// Fire connection events
						switch (status)
						{
							case NetConnectionStatus.Connected:
								if (!_isServer) Server = msg.SenderConnection;
								NewConnection(this, msg);
								break;
							case NetConnectionStatus.Disconnected:
								if (!_isServer && Server == null)
									FailedConnection(this, msg);
								else
									LostConnection(this, msg);

								if (!_isServer)
								{
									Server = null;
									Peer = null;
									return;
								}

								break;
						}

						break;

					case NetIncomingMessageType.Data:
						PacketHandlers.HandlePacket(msg);
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