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
	public sealed class Texture2D
	{
		private int _height;
		private int _texture;
		private bool _useAlpha;
		private bool _useBgra;
		private int _width;

		/// <summary>
		///     Initializes a new instance of the <see cref="Texture2D" /> class.
		/// </summary>
		/// <param name="path">The path to the image file to use for this Texture2D.</param>
		/// <param name="useAlpha">If set to <c>true</c>, use alpha channel. UNTESTED!</param>
		/// <exception cref="System.Exception">Can't find the image file!</exception>
		public Texture2D(string path, bool useAlpha = false)
		{
			// If the file doesn't exist, we can't do anything
			if (!File.Exists(path)) throw new Exception("Can't find file \"" + path + "\"!");

			// Load the data from the image file
			LoadFromBitmap(new Bitmap(path), useAlpha);
		}

		public Texture2D(int width, int height, bool useAlpha = false, bool useFiltering = true, bool useBgra = false)
		{
			// Create a new bitmap at the size we need
			var bitmap = new Bitmap(width, height);

			// Fill it with a single color
			using (var gfx = System.Drawing.Graphics.FromImage(bitmap))
			using (var brush = new SolidBrush(Color.FromArgb(255, 0, 255)))
			{
				gfx.FillRectangle(brush, 0, 0, width, height);
			}

			// Load the data from the bitmap
			LoadFromBitmap(bitmap, useAlpha, useFiltering, useBgra);
		}

		private void LoadFromBitmap(Bitmap bitmap, bool useAlpha, bool useFiltering = true, bool useBgra = false)
		{
			// TODO: Add contract here to make sure _textureId isn't already pointing to a texture

			// Save some metadata
			_useAlpha = useAlpha;
			_useBgra = useBgra;
			_width = bitmap.Width;
			_height = bitmap.Height;

			// Load the data from the bitmap
			var textureData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly,
				useAlpha ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

			// Generate and bind a new OpenGL texture
			_texture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, _texture);

			// Configure the texture
			GL.TexEnv(TextureEnvTarget.TextureEnv,
				TextureEnvParameter.TextureEnvMode, (float) TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMinFilter, (float)(useFiltering ? TextureMinFilter.Linear : TextureMinFilter.Nearest));
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMagFilter, (float)(useFiltering ? TextureMinFilter.Linear : TextureMinFilter.Nearest));

			// Load the texture
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0, // level
				PixelInternalFormat.Rgba,
				bitmap.Width, bitmap.Height,
				0, // border
				useBgra ? (useAlpha ? GLPixelFormat.Bgra : GLPixelFormat.Bgr) : (useAlpha ? GLPixelFormat.Rgba : GLPixelFormat.Rgb),
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
				0, 0, _width, _height,
				_useBgra ? (_useAlpha ? GLPixelFormat.Bgra : GLPixelFormat.Bgr) : (_useAlpha ? GLPixelFormat.Rgba : GLPixelFormat.Rgb),
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