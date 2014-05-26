using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Structures.Objects
{
	public abstract class ClientObjectBase : IParentable
	{
		public int Id { get; set; }
		public Vector2 Position { get; set; }
		public IPositionable Parent { get; private set; }

		protected ClientObjectBase(ClientStructure parent)
		{
			Parent = parent;
		}

		public abstract void OnUse();

		public virtual void Render()
		{
			// White test squares for now
			GL.Color3(Color.FromArgb(255, 255, 255));

			GL.PushMatrix();
			GL.Translate(Position.X, Position.Y, 0f);
			GL.Begin(PrimitiveType.Quads);

			GL.Vertex2(-0.2f, 0.2f); // Left Top
			GL.Vertex2(-0.2f, -0.2f); // Left Bottom
			GL.Vertex2(0.2f, -0.2f); // Right Bottom
			GL.Vertex2(0.2f, 0.2f); // Right Top

			GL.End();
			GL.PopMatrix();
		}
	}
}