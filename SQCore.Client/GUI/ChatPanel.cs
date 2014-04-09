using System.IO;
using RazorEngine;
using SquareCubed.Client.Gui;

namespace SQCore.Client.GUI
{
	internal class ChatPanel : GuiPanel
	{
		public ChatPanel(Gui gui)
			: base(gui, "Chat")
		{
			var template = File.ReadAllText(@"GUI/Panels/Chat/Panel.cshtml");
			var model = new {Name = "Test"};
			var result = Razor.Parse(template, model);
			//Console.WriteLine(result);
		}
	}
}