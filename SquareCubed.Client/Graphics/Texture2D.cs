using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GLPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace SquareCubed.Client.Graphics
{
	[Flags]
	public enum TextureOptions
	{
		None = 0x0,
		Alpha = 0x1,
		Filtering = 0x2,
		Bgra = 0x4
	}

	public sealed class Texture2D
	{
		private int _texture;

		/// <summary>
		///     Initializes a new instance of the <see cref="Texture2D" /> class
		///     using texture data from a file.
		/// </summary>
		/// <param name="path">The path to the image file to use for this Texture2D.</param>
		/// <param name="options">The option flags to use for this texture.</param>
		/// <exception cref="System.Exception">Can't find the image file!</exception>
		public Texture2D(string path, TextureOptions options = TextureOptions.None)
		{
			// If the file doesn't exist, we can't do anything
			if (!File.Exists(path)) throw new Exception("Can't find texture file \"" + path + "\"!");

			// Load the data from the image file
			LoadFromBitmap(new Bitmap(path), options);
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Texture2D" /> class
		///     using test texture data.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="options">The option flags to use for this texture.</param>
		public Texture2D(int width, int height, TextureOptions options = TextureOptions.None)
		{
			// Create a new bitmap at the size we need
			var bitmap = new Bitmap(width, height);

			// Fill it with a single color
			using (var gfx = System.Drawing.Graphics.FromImage(bitmap))
			{
				using (var brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
				{
					gfx.FillRectangle(brush, 0, 0, width, height);
				}
				using (var brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
				{
					// Draw the loading message
					var font = new Font("Lucida Console", 10, FontStyle.Regular, GraphicsUnit.Point);
					var textSize = gfx.MeasureString("loading", font);
					gfx.DrawString("loading", font, brush,
						(width*0.5f) - (textSize.Width*0.5f),
						(height*0.5f) - (textSize.Height*0.5f));
				}
			}

			// Load the data from the bitmap
			LoadFromBitmap(bitmap, options);
		}

		public int Width { get; private set; }
		public int Height { get; private set; }

		public TextureOptions Options { get; private set; }

		public bool UseAlpha
		{
			get { return (Options & TextureOptions.Alpha) == TextureOptions.Alpha; }
		}

		public bool UseFiltering
		{
			get { return (Options & TextureOptions.Filtering) == TextureOptions.Filtering; }
		}

		public bool UseBgra
		{
			get { return (Options & TextureOptions.Bgra) == TextureOptions.Bgra; }
		}

		private void LoadFromBitmap(Bitmap bitmap, TextureOptions options)
		{
			// Save some metadata
			Options = options;
			Width = bitmap.Width;
			Height = bitmap.Height;

			// Load the data from the bitmap
			var textureData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly,
				UseAlpha ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

			// Generate and bind a new OpenGL texture
			_texture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, _texture);

			// Configure the texture
			GL.TexEnv(TextureEnvTarget.TextureEnv,
				TextureEnvParameter.TextureEnvMode, (float) TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMinFilter, (float) (UseFiltering ? TextureMinFilter.Linear : TextureMinFilter.Nearest));
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMagFilter, (float) (UseFiltering ? TextureMinFilter.Linear : TextureMinFilter.Nearest));

			// Load the texture
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0, // level
				PixelInternalFormat.Rgba,
				bitmap.Width, bitmap.Height,
				0, // border
				UseBgra
					? (UseAlpha ? GLPixelFormat.Bgra : GLPixelFormat.Bgr)
					: (UseAlpha ? GLPixelFormat.Rgba : GLPixelFormat.Rgb),
				PixelType.UnsignedByte,
				textureData.Scan0);

			// Free the data since we won't need it anymore
			bitmap.UnlockBits(textureData);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void MapSubImage(IntPtr pixels)
		{
			GL.BindTexture(TextureTarget.Texture2D, _texture);
			GL.TexSubImage2D(
				TextureTarget.Texture2D, 0,
				0, 0, Width, Height,
				UseBgra
					? (UseAlpha ? GLPixelFormat.Bgra : GLPixelFormat.Bgr)
					: (UseAlpha ? GLPixelFormat.Rgba : GLPixelFormat.Rgb),
				PixelType.UnsignedByte, pixels);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		/// <summary>
		///     Sets the texture as active and creates a new lifetime
		///     object to deactivate the texture once done.
		/// </summary>
		/// <returns>A new texture lifetime object that should be disposed when done.</returns>
		[Pure]
		public ActivationLifetime Activate()
		{
			return new ActivationLifetime(_texture);
		}

		public void Render(Vector2 position, Vector2 size)
		{
			using (Activate())
			{
				GL.Begin(PrimitiveType.Quads);
				GL.Color3(Color.White);

				// Left Bottom
				GL.TexCoord2(0, 1);
				GL.Vertex2(position.X, position.Y);

				// Right Bottom
				GL.TexCoord2(1, 1);
				GL.Vertex2(position.X + size.X, position.Y);

				// Right Top
				GL.TexCoord2(1, 0);
				GL.Vertex2(position.X + size.X, position.Y + size.Y);

				// Left Top
				GL.TexCoord2(0, 0);
				GL.Vertex2(position.X, position.Y + size.Y);

				GL.End();
			}
		}

		public sealed class ActivationLifetime : IDisposable
		{
			public ActivationLifetime(int texture)
			{
				GL.ActiveTexture(TextureUnit.Texture0);
				GL.BindTexture(TextureTarget.Texture2D, texture);
			}

			public void Dispose()
			{
				GL.ActiveTexture(TextureUnit.Texture0);
				GL.BindTexture(TextureTarget.Texture2D, 0);
			}
		}
	}
}