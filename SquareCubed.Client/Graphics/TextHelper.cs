using System.Drawing;
using System.Drawing.Text;

namespace SquareCubed.Client.Graphics
{
	public static class TextHelper
	{
		private static readonly StringFormat StringFormat;

		static TextHelper()
		{
			// This probably should be done better somehow at some point.
			// GenericTypographic doesn't pad out the text at the end and start
			StringFormat = new StringFormat(StringFormat.GenericTypographic);
			StringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
		}

		public static Size MeasureString(string text, Font font)
		{
			using (var img = new Bitmap(1, 1))
			using (var gfx = System.Drawing.Graphics.FromImage(img))
			{
				gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
				return gfx.MeasureString(text, font, int.MaxValue, StringFormat).ToSize();
			}
		}

		public static Texture2D RenderString(string text, int textSize, Color textColor)
		{
			var font = new Font("Segoe UI", textSize, FontStyle.Regular, GraphicsUnit.Pixel);

			var size = MeasureString(text, font);
			var img = new Bitmap(size.Width + 1, size.Height); // + 1 is because anti aliasing will make it 1 off sometimes
			using (var gfx = System.Drawing.Graphics.FromImage(img))
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