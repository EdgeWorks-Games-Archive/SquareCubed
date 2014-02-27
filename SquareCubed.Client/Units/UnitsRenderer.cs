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

				GL.Color3(Color.WhiteSmoke);
				GL.Vertex2(-0.3f, 0.3f); // Left Top
				GL.Vertex2(-0.3f, -0.3f); // Left Bottom
				GL.Vertex2(0.3f, -0.3f); // Right Bottom
				GL.Vertex2(0.3f, 0.3f); // Right Top

				GL.End();

				GL.PopMatrix();
			}
		}
	}
}
