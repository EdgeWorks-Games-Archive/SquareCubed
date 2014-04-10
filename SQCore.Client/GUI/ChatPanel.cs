using System.IO;
using RazorEngine;
using SquareCubed.Client.Gui;

namespace SQCore.Client.Gui
{
	internal class ChatPanel : GuiPanel
	{
		public ChatPanel(SquareCubed.Client.Gui.Gui gui)
			: base(gui, "Chat")
		{
			var template = File.ReadAllText(@"GUI/Panels/Chat/Panel.cshtml");
			var model = new {Name = "Test"};
			var result = Razor.Parse(template, model);
			//Console.WriteLine(result);
		}
	}
}