using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SquareCubed.Client.Gui.Data
{
	public class Script
	{
		[XmlAttribute("src")]
		public string Source { get; set; }
	}

	public class Style
	{
		[XmlAttribute("src")]
		public string Source { get; set; }
	}

	public class Panel
	{
		[XmlAttribute("src")]
		public string Source { get; set; }

		[XmlArrayItem]
		public List<Script> Scripts { get; set; }

		[XmlArrayItem]
		public List<Style> Styles { get; set; }
	}
}