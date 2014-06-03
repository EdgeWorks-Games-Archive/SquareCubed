using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Gui.Controls
{
	class GuiTest : GuiControl
	{
		public override void Render()
		{
			GL.Begin(PrimitiveType.Quads);

			// Border
			GL.Color3(Color.White);
			GL.Vertex2(Position.X, Position.Y);
			GL.Vertex2(Position.X, Position.Y + 10);
			GL.Vertex2(Position.X + 10, Position.Y + 10);
			GL.Vertex2(Position.X + 10, Position.Y);

			GL.End();
		}
	}
}
