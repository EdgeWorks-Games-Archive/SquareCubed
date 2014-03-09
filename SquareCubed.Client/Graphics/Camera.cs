using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Graphics
{
	public class Camera
	{
		#region Resolution and Size

		private Size _res;

		private SizeF _size;

		public SizeF Size
		{
			get { return _size; }
		}

		public void SetHeight(float height)
		{
			_size.Height = height;
			_size.Width = _size.Height * _res.GetRatio();
		}

		#endregion

		#region Positioning, Following and Parenting

		public Vector2 Position { get; set; }
		public IPositionable Parent { get; set; }

		#endregion

		public Camera(Size resolution)
		{
			_res = resolution;

			// Set Default Size
			SetHeight(14);
		}


		public void SetMatrices()
		{
			// Set up camera size
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(
				-Size.Width/2.0f, Size.Width/2.0f,
				-Size.Height/2.0f, Size.Height/2.0f,
				0.0f, 4.0f);

			// Now set up the camera position
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			// Read the following from bottom to top, I know it's confusing
			// and I don't know why it works but it does and I'm too frustrated
			// to EVER touch this EVER again unless I get a really really good
			// explanation of what the frigging frack I'm looking at.

			// Move to camera position
			GL.Translate(
				-Position.X,
				-Position.Y,
				0.0f);

			// If no parent, we're done here
			if (Parent == null) return;

			// Then Move to the Parent's Local 0,0
			GL.Translate(
				Parent.Center.X,
				Parent.Center.Y,
				0.0f);

			// Rotate around the Parent's Center
			GL.Rotate(Parent.Rotation, 0, 0, 1);

			// Move to Parent Center
			GL.Translate(
				-Parent.Position.X,
				-Parent.Position.Y,
				0.0f);
		}
	}
}