using OpenTK;

namespace SquareCubed.Client.Gui.Controls
{
	public abstract class GuiForm : GuiControl
	{
		public Vector2 Position { get; set; }
		public string Title { get; set; }

		internal void Render()
		{
		}
	}
}