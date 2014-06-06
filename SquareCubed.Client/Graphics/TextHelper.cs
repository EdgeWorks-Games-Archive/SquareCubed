using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

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

		public static Font GetFont(int size)
		{
			return new Font("Segoe UI", size, FontStyle.Regular, GraphicsUnit.Pixel);
		}

		public static Size MeasureString(string text, int textSize)
		{
			// TODO: Overall improve this entire class so it's a bit better designed
			using (var img = new Bitmap(1, 1))
			using (var gfx = System.Drawing.Graphics.FromImage(img))
			{
				gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
				return gfx.MeasureString(text, GetFont(textSize), int.MaxValue, StringFormat).ToSize();
			}
		}

		public static int GetClosestPosition(string text, int textSize, int mousePosition)
		{
			for (var i = 1; i < text.Length + 1; i++)
			{
				// -1 to make clicking in text just a bit easier
				// TODO: Perhaps calculate the offset on a letter by letter basis instead of just -1?
				if (mousePosition < MeasureString(text.Substring(0, i), textSize).Width - 1)
					return i - 1;
			}
			return text.Length;
		}

		public static Texture2D RenderString(string text, int textSize, Color textColor)
		{
			// Prevents crash or incorrect rendering in case of empty string
			if (text == "")
				text = " ";

			var font = GetFont(textSize);
			var size = MeasureString(text, textSize);
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