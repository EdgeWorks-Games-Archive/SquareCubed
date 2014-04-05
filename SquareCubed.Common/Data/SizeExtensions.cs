using System.Drawing;

namespace SquareCubed.Common.Data
{
	public static class SizeExtensions
	{
		/// <summary>
		///     Returns the ration of the width to the height.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static float GetRatio(this Size size)
		{
			return ((float) size.Width)/size.Height;
		}
	}
}