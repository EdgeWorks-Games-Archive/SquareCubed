using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Graphics
{
	public class Camera
	{
		#region Resolution, Size and Position

		private Size _res;
		private SizeF _size;

		public Size Resolution
		{
			get { return _res; }
		}

		public SizeF Size
		{
			get { return _size; }
		}

		public float Width
		{
			get { return _size.Width; }
		}

		public float AspectRatio
		{
			get { return _res.GetRatio(); }
		}

		public float Height
		{
			get { return _size.Height; }
			set
			{
				_size.Height = value;
				_size.Width = _size.Height*_res.GetRatio();
			}
		}

		public Vector2 Position { get; set; }
		public IComplexPositionable Parent { get; set; }
		public bool PilotMode { get; set; }

		#endregion

		public Camera(Size resolution)
		{
			_res = resolution;

			// Set Default Size
			Height = 14;
		}

		public void SetMatrices()
		{
			// Set up camera size
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(
				-Width/2.0f, Width/2.0f,
				-Height/2.0f, Height/2.0f,
				0.0f, 4.0f);

			// Now set up the camera position
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			// Read the following from bottom to top, I know it's confusing
			// and I don't know why it works but it does and I'm too frustrated
			// to EVER touch this EVER again unless I get a really really good
			// explanation of what the frigging frack I'm looking at.

			if (!PilotMode)
			{
				// If we're not in pilot mode, we need to listen to the position set to the camera

				// Offset to camera position
				GL.Translate(
					-Position.X,
					-Position.Y,
					0.0f);
			}
			else
			{
				// If we are in pilot mode, we need to lock to the parent's center and rotate back

				// Rotate back to 0 rotation
				GL.Rotate(MathHelper.RadiansToDegrees(Parent.Rotation), 0, 0, 1);

				// Move to local center of parent
				GL.Translate(
					-Parent.LocalCenter.X,
					-Parent.LocalCenter.Y,
					0.0f);
			}

			// If no parent, we can't do anything more
			if (Parent == null) return;

			// Rotate around the Parent's 0,0
			GL.Rotate(-MathHelper.RadiansToDegrees(Parent.Rotation), 0, 0, 1);

			// Move to Parent Local 0,0
			GL.Translate(
				-Parent.Position.X,
				-Parent.Position.Y,
				0.0f);
		}

		public Vector2 AbsoluteToRelative(Vector2i absolute)
		{
			// Set the relative position to the center of the camera
			var relative = new Vector2(Position.X, Position.Y);

			// Add the offset that the absolute is from the center of the camera to the relative
			// TODO: improve to not divide every time used
			relative.X += _size.Width * ((float)absolute.X / _res.Width) - (_size.Width / 2);
			relative.Y -= _size.Height * ((float)absolute.Y / _res.Height) - (_size.Height / 2);

			return relative;
		}
	}
}