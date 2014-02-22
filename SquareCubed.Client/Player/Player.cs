using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Player
{
	public class Player
	{
		private const float Speed = 2;
		private readonly Client _client;
		private Vector2 _position;

		public Player(Client client)
		{
			_client = client;
		}

		public void Update(float delta)
		{
			_position += _client.Input.Axes*delta*Speed;
		}

		public void Render(float delta)
		{
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.Translate(_position.X, _position.Y, 0);

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