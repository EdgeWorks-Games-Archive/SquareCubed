using System.Drawing;
using System.Drawing.Text;
using SGraphics = System.Drawing.Graphics;

namespace SquareCubed.Client.Graphics
{
	public sealed partial class Texture2D
	{
		private static readonly StringFormat StringFormat;

		static Texture2D()
		{
			// This probably should be done better somehow at some point.
			// GenericTypographic doesn't pad out the text at the end and start
			StringFormat = new StringFormat(StringFormat.GenericTypographic);
			StringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
		}

		private static Size MeasureString(string text, Font font)
		{
			using (var img = new Bitmap(1, 1))
			using (var gfx = SGraphics.FromImage(img))
			{
				gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
				return gfx.MeasureString(text, font, int.MaxValue, StringFormat).ToSize();
			}
		}

		public static Texture2D FromText(string text, int textSize, Color textColor)
		{
			var font = new Font("Segoe UI", textSize, FontStyle.Regular, GraphicsUnit.Pixel);

			var size = MeasureString(text, font);
			var img = new Bitmap(size.Width, size.Height);
			using (var gfx = SGraphics.FromImage(img))
			{
				// Thanks to GWEN.NET for the following information:
				// NOTE:    TextRenderingHint.AntiAliasGridFit looks sharper and in most cases better
				//          but it comes with a some problems.
				//
				//          1.  Graphic.MeasureString and format.MeasureCharacterRanges 
				//              seem to return wrong values because of this.
				//
				//          2.  While typing the kerning changes in random places in the sentence.
				// 
				//          Until 1st problem is fixed we should use TextRenderingHint.AntiAlias...  :-(
				gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
				gfx.DrawString(text, font, new SolidBrush(textColor), 0, 0, StringFormat);
			}
			return new Texture2D(img);
			// TODO: Make Texture2D stop disposing the bitmap so it can be made part of the using block
		}
	}
}