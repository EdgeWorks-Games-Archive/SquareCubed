using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Data;
using SquareCubed.Utils;

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

		#region Position and Parenting

		public Vector2 Position { get; set; }
		public IParentable Parent { get; set; }

		#endregion

		public Camera(Size resolution)
		{
			_res = resolution;

			// Set Default Size
			SetHeight(14);
		}


		public void SetProjectionMatrix()
		{
			// Set up camera size
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(
				-Size.Width/2.0f, Size.Width/2.0f,
				-Size.Height/2.0f, Size.Height/2.0f,
				0.0f, 4.0f);

			if (Parent != null)
			{
				// Move to Parent Center, Rotate around it, then Move to the Parent 0, 0
				GL.Rotate(Parent.Rotation, 0, 0, 1);
				GL.Translate(
					-Parent.Position.X,
					-Parent.Position.Y,
					0.0f);
			}

			// Move to camera position
			GL.Translate(
				-Position.X,
				-Position.Y,
				0.0f);
		}
	}
}