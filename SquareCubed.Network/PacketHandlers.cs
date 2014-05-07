using System;
using System.Collections.Generic;
using Lidgren.Network;
using SquareCubed.Common.Utils;

namespace SquareCubed.Network
{
	public class PacketHandlers
	{
		private readonly Dictionary<int, Action<NetIncomingMessage>> _entries =
			new Dictionary<int, Action<NetIncomingMessage>>();

		private readonly Logger _logger = new Logger("Packets");
		
		public void HandlePacket(NetIncomingMessage msg)
		{
#if !DEBUG
			try
			{
#endif
			TriggerHandler(msg.ReadInt32(), msg);
#if !DEBUG
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

		public void TriggerHandler(int type, NetIncomingMessage msg)
		{
			// If we have a packet handler on this type, invoke it, if not throw to drop client
			Action<NetIncomingMessage> handler;
			if (_entries.TryGetValue(type, out handler))
				handler(msg);
			else
			{
				var error = string.Format("Connection sent invalid packet type {0}!", type);
				_logger.LogInfo(error);
				throw new InvalidOperationException(error);
			}
		}

		public void Bind(PacketType type, Action<NetIncomingMessage> handler)
		{
			// Check requirements
			if (_entries.ContainsKey(type.Id))
				throw new InvalidOperationException("Handler for type already registered!");

			_entries.Add(type.Id, handler);
		}

		public void Unbind(PacketType type)
		{
			// Check requirements
			if (!_entries.ContainsKey(type.Id))
				throw new InvalidOperationException("Handler for type not registered!");

			_entries.Remove(type.Id);
		}
	}
}