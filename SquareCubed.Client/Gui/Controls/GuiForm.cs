using System.Drawing;
using OpenTK;

namespace SquareCubed.Client.Gui.Controls
{
	public abstract class GuiForm : GuiControl
	{
		public Point Position { get; set; }
		public Size Size
		{
			// Size + 1px Border (x2)
			get { return InnerSize + new Size(2, 2); }
			set { InnerSize = value - new Size(2, 2); }
		}
		public string Title { get; set; }
		public Size InnerSize { get; set; }

		internal void Render()
		{
		}
	}
}