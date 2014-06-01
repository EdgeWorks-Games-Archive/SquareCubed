using OpenTK;

namespace SquareCubed.Client.Gui.Components
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