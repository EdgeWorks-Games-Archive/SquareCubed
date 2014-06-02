using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using SGraphics = System.Drawing.Graphics;

namespace SquareCubed.Client.Graphics
{
	public sealed partial class Texture2D
	{
		public static Texture2D FromText(string text, int textSize, Color textColor)
		{
			var font = new Font("Segoe UI", textSize, FontStyle.Regular, GraphicsUnit.Pixel);
			var size = TextRenderer.MeasureText(text, font);

			var img = new Bitmap(size.Width, size.Height);
			using (var gfx = SGraphics.FromImage(img))
			{
				gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				gfx.DrawString(text, font, new SolidBrush(textColor), 0, 0);
			}
			return new Texture2D(img);
		}
	}
}