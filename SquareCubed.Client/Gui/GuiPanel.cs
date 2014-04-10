using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Web.UI;
using System.Xml.Serialization;
using RazorEngine;
using SquareCubed.Client.Gui.Data;

namespace SquareCubed.Client.Gui
{
	public class GuiPanel
	{
		/// <summary>
		///     Counter to make sure every panel has a unique Id.
		/// </summary>
		private static int _counter;

		private readonly string _id;

		/// <summary>
		///     Initializes a new instance of the <see cref="GuiPanel" /> class.
		///     The panel will be loaded from GUI/Panels/[name]/Panel.xml.
		/// </summary>
		/// <param name="gui">A reference to the main Gui object.</param>
		/// <param name="name">The identifier name of the panel.</param>
		protected GuiPanel(Gui gui, string name)
		{
			Contract.Requires<ArgumentNullException>(gui != null);

			// Generate data about the panel before we load it in
			_id = "panel-" + _counter;
			var urlRoot = "Panels/" + name + "/";
			var folderRoot = "GUI/Panels/" + name + "/";

			// Load in the panel data
			var serializer = new XmlSerializer(typeof (Panel));
			var fileStream = new FileStream(folderRoot + "Panel.xml", FileMode.Open);
			var panelData = (Panel) serializer.Deserialize(fileStream);

			// Generate the Html and add it
			using (var stringWriter = new StringWriter())
			using (var writer = new HtmlTextWriter(stringWriter))
			{
				// Start rendering the panel div
				writer.AddAttribute(HtmlTextWriterAttribute.Id, _id);
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel");
				writer.RenderBeginTag(HtmlTextWriterTag.Div);

				// Render and write the actual panel
				var template = File.ReadAllText(folderRoot + panelData.Source);
				var result = Razor.Parse(template);
				writer.Write(result);

				// Finish rendering the panel div and send it to the view
				writer.RenderEndTag();
				gui.AddHtml(stringWriter.ToString());
			}

			// Add the scripts
			panelData.Scripts.ForEach(s => gui.AddScript(urlRoot + s.Source));

			// Increment the counter so every Id is identical
			_counter++;
		}
	}
}