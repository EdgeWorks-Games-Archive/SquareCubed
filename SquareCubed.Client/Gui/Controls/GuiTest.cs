using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Gui.Controls
{
	class GuiTest : GuiControl
	{
		public override Size Size { get; set; }

		internal override void Render(float delta)
		{
			GL.Begin(PrimitiveType.Quads);

			// White Square
			GL.Color3(Color.White);
			GL.Vertex2(Position.X, Position.Y);
			GL.Vertex2(Position.X, Position.Y + Size.Height);
			GL.Vertex2(Position.X + Size.Width, Position.Y + Size.Height);
			GL.Vertex2(Position.X + Size.Width, Position.Y);

			GL.End();
		}
	}
}
