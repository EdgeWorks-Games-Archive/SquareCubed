using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Units
{
	class UnitsRenderer
	{
		public void RenderUnits(IEnumerable<Unit> units)
		{
			GL.MatrixMode(MatrixMode.Modelview);
			foreach (var unit in units)
			{
				GL.PushMatrix();
				GL.Translate(unit.Position.X, unit.Position.Y, 0);

				// Render Test Quad to represent Player Position
				GL.Begin(PrimitiveType.Quads);

				GL.Color3(Color.Red);
				GL.Vertex2(-0.4f, 0.4f); // Left Top
				GL.Color3(Color.Lime);
				GL.Vertex2(-0.4f, -0.4f); // Left Bottom
				GL.Color3(Color.Blue);
				GL.Vertex2(0.4f, -0.4f); // Right Bottom
				GL.Color3(Color.Yellow);
				GL.Vertex2(0.4f, 0.4f); // Right Top

				GL.End();

				GL.PopMatrix();
			}
		}
	}
}
