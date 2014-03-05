using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SQCore.Client
{
	internal class StarsBackground
	{
		private readonly Random _random = new Random();
		private readonly Vector2 _resolution;
		private readonly List<StarData> _starData = new List<StarData>();

		public StarsBackground(SquareCubed.Client.Client client)
		{
			_resolution = new Vector2(client.Window.Width, client.Window.Height);

			// Generate star data
			for (var i = 0; i < 200; i++)
			{
				// Generate a new star
				_starData.Add(new StarData
				{
					Position = new Vector2(
						(float) _random.NextDouble()*_resolution.X,
						(float) _random.NextDouble()*_resolution.Y),
					Rotation = (float) _random.NextDouble()*360.0f,
					Scale = (float) _random.NextDouble()*1.0f + 0.5f
				});
			}

			// Bind rendering event
			client.BackgroundRenderTick += Render;
		}

		private void Render(object sender, float e)
		{
			// Temporarily set the camera to something more convenient
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.Ortho(
				0, _resolution.X,
				0, _resolution.Y,
				0.0, 4.0);

			// Draw all the stars
			GL.MatrixMode(MatrixMode.Modelview);
			foreach (var star in _starData)
			{
				GL.PushMatrix();

				// Move and rotate to the correct position
				GL.Translate(star.Position.X, star.Position.Y, 0);
				GL.Rotate(star.Rotation, 0.0f, 0.0f, 1.0f);
				GL.Scale(star.Scale, star.Scale, star.Scale);

				// Draw a white square
				GL.Begin(PrimitiveType.Quads);
				GL.Color3(Color.White);

				GL.Vertex2(-1, -1); // Left Bottom
				GL.Vertex2(1, -1); // Right Bottom
				GL.Vertex2(1, 1); // Right Top
				GL.Vertex2(-1, 1); // Left Top

				GL.End();

				GL.PopMatrix();
			}

			// Reset the resolution
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
		}

		private class StarData
		{
			public Vector2 Position { get; set; }
			public float Rotation { get; set; }
			public float Scale { get; set; }
		}
	}
}