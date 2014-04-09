using System;
using System.IO;
using RazorEngine;
using SquareCubed.Client.Gui;

namespace SQCore.Client.Chat
{
	internal class Chat
	{
		private GuiChatPanel _panel;

		public Chat(Gui gui)
		{
			_panel = new GuiChatPanel(gui);

			var template = File.ReadAllText(@"GUI/Panels/Chat/Panel.cshtml");
			var model = new { Name = "Test" };
			var result = Razor.Parse(template, model);
			Console.WriteLine(result);
		}
	}
}