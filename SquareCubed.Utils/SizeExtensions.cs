using System.Drawing;

namespace SquareCubed.Utils
{
	public static class SizeExtensions
	{
		public static float GetRatio(this Size size)
		{
			return (float) size.Width/size.Height;
		}
	}
}