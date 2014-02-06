using System.Drawing;
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

		#endregion

		public Camera(Size resolution)
		{
			_res = resolution;

			// Set Default Size
			_size = new SizeF(1, 1);
			SetHeight(1);
		}


		public void SetHeight(float height)
		{
			_size.Width = height*_res.GetRatio();
		}
	}
}