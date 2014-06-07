using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Graphics;

namespace SQCore.Client.Background
{
	class SpaceRenderer
	{
		private readonly Camera _camera;

		public SpaceRenderer(Camera camera)
		{
			_camera = camera;
		}

		public void Render(IEnumerable<StarData> starData, float fieldSize, Size resolution)
		{
			// Clear to the background color
			GL.ClearColor(Color.FromArgb(5, 5, 8));
			GL.Clear(ClearBufferMask.ColorBufferBit);

			// Temporarily set the camera to something more convenient
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.Ortho(
				-(resolution.Width * 0.5f), resolution.Width * 0.5f,
				-(resolution.Height * 0.5f), resolution.Height * 0.5f,
				0.0, 4.0);

			// Offset it so it's in the middle of the field
			if (_camera.Parent != null && !_camera.PilotMode) GL.Rotate(MathHelper.RadiansToDegrees(_camera.Parent.Rotation), 0, 0, 1);
			GL.Translate(
				-(fieldSize * 0.5f),
				-(fieldSize * 0.5f),
				0.0f);

			// Reset ModelView to default
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();

			// Draw all the stars
			foreach (var star in starData)
			{
				RenderStar(star);
			}

			// Restore ModelView
			GL.PopMatrix();

			// Reset the resolution
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
		}

		private void RenderStar(StarData star)
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
				case ColorShiftDirection.White:
				case ColorShiftDirection.White2:
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
	}
}
