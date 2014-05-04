using SquareCubed.Client.Gui;
using SquareCubed.Common.Utils;
using SquareCubed.Network;

namespace SQCore.Client.Chat
{
	internal class Chat : GuiPanel
	{
		private readonly ChatNetwork _network;
		private readonly Logger _logger = new Logger("Chat");

		public Chat(SquareCubed.Client.Gui.Gui gui, Network network)
			: base(gui, "Chat")
		{
			_network = new ChatNetwork(network, this);

			Gui.BindCall<string>("chat.send", Send);
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
			Gui.Trigger("chat.message", player, message);
		}
	}
}