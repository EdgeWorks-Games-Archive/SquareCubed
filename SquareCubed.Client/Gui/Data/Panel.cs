using System.Xml.Serialization;

namespace SquareCubed.Client.Gui.Data
{
	public class Panel
	{
		[XmlAttribute("src")]
		public string Source { get; set; }
	}
}