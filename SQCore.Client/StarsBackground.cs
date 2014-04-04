using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client;
using SquareCubed.Client.Graphics;

namespace SQCore.Client
{
	internal class StarsBackground
	{
		private readonly Camera _camera;
		private readonly float _fieldSize;
		private readonly Random _random = new Random();
		private readonly Vector2 _resolution;
		private readonly List<StarData> _starData = new List<StarData>();

		public StarsBackground(SquareCubed.Client.Client client)
		{
			_resolution = new Vector2(client.Window.Width, client.Window.Height);
			_camera = client.Graphics.Camera;

			// Calculate the size of the star field (so we can rotate freely)
			// It's only a float and not a vector because the field will always be square
			var max = Math.Max(_resolution.X, _resolution.Y);
			var min = Math.Min(_resolution.X, _resolution.Y);
			_fieldSize = (float) Math.Sqrt((max*max) + (min*min));

			// Calculate volume and with that amount of stars
			var volume = _fieldSize*_fieldSize;
			var starCount = volume/2500.0f; // Per 100 volume, 1 star

			// Generate star data
			for (var i = 0; i < starCount; i++)
			{
				// Generate a new star
				_starData.Add(new StarData
				{
					Position = new Vector2(
						(float) _random.NextDouble()*_fieldSize,
						(float) _random.NextDouble()*_fieldSize),
					Rotation = (float) _random.NextDouble()*360.0f,
					Scale = (float) _random.NextDouble()*1.0f + 0.5f,
					ColorShiftDirection = (ColorShiftDirection) _random.Next(0, 4),
					ColorShiftMagnitude = (float) _random.NextDouble()*0.2f
				});
			}

			// Bind rendering event
			client.BackgroundRenderTick += Render;
		}

		private void Render(object sender, TickEventArgs e)
		{
			// Temporarily set the camera to something more convenient
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.Ortho(
				-(_resolution.X/2.0f), _resolution.X/2.0f,
				-(_resolution.Y/2.0f), _resolution.Y/2.0f,
				0.0, 4.0);

			// Offset it so it's in the middle of the field
			if (_camera.Parent != null) GL.Rotate(_camera.Parent.Rotation, 0, 0, 1);
			GL.Translate(
				-(_fieldSize/2.0f),
				-(_fieldSize/2.0f),
				0.0f);

			// Reset ModelView to default
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();

			// Draw all the stars
			foreach (var star in _starData)
			{
				GL.PushMatrix();

				// Move and rotate to the correct position
				GL.Translate(star.Position.X, star.Position.Y, 0);
				GL.Rotate(star.Rotation, 0.0f, 0.0f, 1.0f);
				GL.Scale(star.Scale, star.Scale, star.Scale);

				// Draw a white square
				GL.Begin(PrimitiveType.Quads);
				switch (star.ColorShiftDirection)
				{
					case ColorShiftDirection.White2:
					case ColorShiftDirection.White:
						GL.Color3(Color.White);
						break;
					case ColorShiftDirection.Red:
						GL.Color3(
							0.80f + star.ColorShiftMagnitude,
							0.70f,
							0.60f);
						break;
					case ColorShiftDirection.Blue:
						GL.Color3(
							0.60f,
							0.70f,
							0.80f + star.ColorShiftMagnitude);
						break;
					default:
						throw new Exception("Unknown star color shift direction!");
				}

				GL.Vertex2(-1, -1); // Left Bottom
				GL.Vertex2(1, -1); // Right Bottom
				GL.Vertex2(1, 1); // Right Top
				GL.Vertex2(-1, 1); // Left Top

				GL.End();

				GL.PopMatrix();
			}

			// Restore ModelView
			GL.PopMatrix();

			// Reset the resolution
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
		}

		private enum ColorShiftDirection
		{
			White,
			White2,
			Red,
			Blue
		}

		private class StarData
		{
			public Vector2 Position { get; set; }
			public float Rotation { get; set; }
			public float Scale { get; set; }
			public ColorShiftDirection ColorShiftDirection { get; set; }
			public float ColorShiftMagnitude { get; set; }
		}
	}
}