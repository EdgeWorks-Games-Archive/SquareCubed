using System.Drawing;
using OpenTK.Graphics.OpenGL;
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

		public Camera(Size resolution)
		{
			_res = resolution;

			// Set Default Size
			SetHeight(16);
		}


		public void SetProjectionMatrix()
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(
				-Size.Width/2, Size.Width/2,
				-Size.Height/2, Size.Height/2,
				0.0, 4.0);
		}
	}
}