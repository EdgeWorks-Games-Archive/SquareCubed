using System;
using System.Linq;
using Lidgren.Network;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Network
{
	public class PacketHandlers
	{
		private readonly Logger _logger = new Logger("Packets");
		private readonly PacketType[] _packetHandlers = new PacketType[20];

		public void HandlePacket(NetIncomingMessage msg, bool isServer)
		{
#if _DEBUG
			try
			{
#endif
			// Read the packet type
			var type = msg.PeekInt16();

			// If we have a packet handler on this type, invoke it, if not throw to drop client
			if (_packetHandlers[type] != null)
				_packetHandlers[type].Invoke(this, msg);
			else
			{
				if (isServer)
					throw new Exception(string.Format("Connection sent invalid packet type {0}!", type));
				_logger.LogInfo("Connection sent invalid packet type {0}!", type);
			}
#if _DEBUG
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
#endif
		}

		private class PacketType
		{
			public PacketType(string id, ushort num)
			{
				Id = id;
				Num = num;
			}

			public string Id { get; private set; }
			public ushort Num { get; private set; }
			public bool IsBound { get { return Handler != null; } }
			public event EventHandler<NetIncomingMessage> Handler;

			public void Invoke(object sender, NetIncomingMessage msg)
			{
				if (Handler != null) Handler(sender, msg);
			}
		}

		#region Packet Type Resolving

		public void RegisterTypeId(string id, ushort num)
		{
			// Make sure not already registered
			if (_packetHandlers.Any(h => h != null && (h.Id == id || h.Num == num)))
				throw new Exception("Handler ID or numeric value already registered!");

			// Register it
			_packetHandlers[num] = new PacketType(id, num);
		}

		public ushort RegisterTypeId(string id)
		{
			// Make sure not already registered
			if (_packetHandlers.Any(h => h != null && h.Id == id))
				throw new Exception("Handler ID already registered!");

			// Find an unused Id
			for (ushort i = 0; i < _packetHandlers.Length; i++)
			{
				if (_packetHandlers[i] != null) continue;

				// Register it
				_packetHandlers[i] = new PacketType(id, i);
				return i;
			}

			// Failed to find an unused Id
			throw new Exception("Packet handlers array full!");
		}

		public ushort ResolveType(string id)
		{
			try
			{
				return _packetHandlers.First(h => h != null && h.Id == id).Num;
			}
			catch
			{
				throw new Exception("Packet type not registered.");
			}
		}

		#endregion

		#region Handler Binding

		public void Bind(string id, EventHandler<NetIncomingMessage> handler)
		{
			try
			{
				var type = _packetHandlers.First(h => h != null && h.Id == id);
				if (type.IsBound) throw new Exception();
				type.Handler += handler;
			}
			catch
			{
				throw new Exception("Packet type not registered or already bound.");
			}
		}

		public void Bind(ushort num, EventHandler<NetIncomingMessage> handler)
		{
			try
			{
				var type = _packetHandlers.First(h => h != null && h.Num == num);
				if (type.IsBound) throw new Exception();
				type.Handler += handler;
			}
			catch
			{
				throw new Exception("Packet type not registered or already bound.");
			}
		}

		#endregion
	}
}