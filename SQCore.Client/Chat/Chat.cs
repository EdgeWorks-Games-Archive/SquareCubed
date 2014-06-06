using System;
using SquareCubed.Common.Utils;
using SquareCubed.Network;

namespace SQCore.Client.Chat
{
	internal sealed class Chat : IDisposable
	{
		private readonly ChatNetwork _network;
		private readonly Logger _logger = new Logger("Chat");

		public Chat(Network network)
		{
			_network = new ChatNetwork(network, this);
		}

		public void Send(string message)
		{
			// Trim whitespace if needed
			message = message.Trim();

			// Make sure you can't send an empty message
			if (message == "")
				return;

			// Log and send the message
			_logger.LogInfo("Sending chat message \"{0}\"...", message);
			_network.SendChatMessage(message);
		}

		public void OnChatMessage(string player, string message)
		{
			_logger.LogInfo("{0}: {1}", player, message);
			//OldGui.Trigger("chat.message", player, message);
		}

		public void Dispose()
		{
		}
	}
}