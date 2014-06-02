using System.Drawing;
using System.Windows.Forms;
using SGraphics = System.Drawing.Graphics;

namespace SquareCubed.Client.Graphics
{
	public sealed partial class Texture2D
	{
		public static Texture2D FromText(string text, int textSize, Brush textBrush)
		{
			var font = new Font("Segoe UI", textSize, FontStyle.Regular, GraphicsUnit.Point);
			var size = TextRenderer.MeasureText(text, font);

			var img = new Bitmap(size.Width, size.Height);
			using (var gfx = SGraphics.FromImage(img))
			{
				gfx.DrawString(text, font, textBrush, 0, 0);
			}
			return new Texture2D(img);
		}
	}
}
