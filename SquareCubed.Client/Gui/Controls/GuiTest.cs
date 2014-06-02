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
			GL.Vertex2(0, 0);
			GL.Vertex2(0, 10);
			GL.Vertex2(10, 10);
			GL.Vertex2(10, 0);

			GL.End();
		}
	}
}
