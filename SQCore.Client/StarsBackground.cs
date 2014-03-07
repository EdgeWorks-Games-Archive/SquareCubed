using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Graphics;

namespace SQCore.Client
{
	internal class StarsBackground
	{
		private readonly Random _random = new Random();
		private readonly Vector2 _resolution;
		private readonly float _fieldSize;
		private readonly Camera _camera;
		private readonly List<StarData> _starData = new List<StarData>();
		
		public StarsBackground(SquareCubed.Client.Client client)
		{
			_resolution = new Vector2(client.Window.Width, client.Window.Height);
			_camera = client.Graphics.Camera;

			// Calculate the size of the star field (so we can rotate freely)
			// It's only a float and not a vector because the field will always be square
			var max = Math.Max(_resolution.X, _resolution.Y);
			var min = Math.Min(_resolution.X, _resolution.Y);
			_fieldSize = (float)Math.Sqrt((max * max) + (min * min));

			// Generate star data
			for (var i = 0; i < 300; i++)
			{
				// Generate a new star
				_starData.Add(new StarData
				{
					Position = new Vector2(
						(float)_random.NextDouble() * _fieldSize,
						(float)_random.NextDouble() * _fieldSize),
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
				-(_resolution.X / 2.0f), _resolution.X / 2.0f,
				-(_resolution.Y / 2.0f), _resolution.Y / 2.0f,
				0.0, 4.0);

			// Offset it so it's in the middle of the field
			if (_camera.Parent != null) GL.Rotate(_camera.Parent.Rotation, 0, 0, 1);
			GL.Translate(
				-(_fieldSize / 2.0f),
				-(_fieldSize / 2.0f),
				0.0f);

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