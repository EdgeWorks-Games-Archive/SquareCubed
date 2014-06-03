using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
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
		Filtering = 0x1,
		Bgra = 0x2
	}

	public sealed partial class Texture2D : IDisposable
	{
		private readonly int _texture;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public Size Size
		{
			get { return new Size(Width, Height); }
		}

		public TextureOptions Options { get; private set; }

		public bool UseFiltering
		{
			get { return (Options & TextureOptions.Filtering) == TextureOptions.Filtering; }
		}

		public bool UseBgra
		{
			get { return (Options & TextureOptions.Bgra) == TextureOptions.Bgra; }
		}

		#region Initialization and Cleanup

		/// <summary>
		///     Initializes a new instance of the <see cref="Texture2D" /> class
		///     using texture data from a file.
		/// </summary>
		/// <param name="path">The path to the image file to use for this Texture2D.</param>
		/// <param name="options">The option flags to use for this texture.</param>
		/// <exception cref="System.Exception">Can't find the image file!</exception>
		public Texture2D(string path, TextureOptions options = TextureOptions.None)
			: this(new Bitmap(path), options)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Texture2D" /> class
		///     using empty texture data at the size given.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="options">The option flags to use for this texture.</param>
		public Texture2D(int width, int height, TextureOptions options = TextureOptions.None)
			: this(new Bitmap(width, height), options)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Texture2D" /> class
		///     using empty texture data at the size given.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="options">The option flags to use for this texture.</param>
		public Texture2D(Size size, TextureOptions options = TextureOptions.None)
			: this(new Bitmap(size.Width, size.Height), options)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Texture2D" /> class
		///     using bitmap data given.
		/// </summary>
		/// <param name="bitmap">The bitmap data.</param>
		/// <param name="options">The option flags to use for this texture.</param>
		public Texture2D(Bitmap bitmap, TextureOptions options = TextureOptions.None)
		{
			Contract.Requires<ArgumentNullException>(bitmap != null);

			// Save some metadata
			Options = options;
			Width = bitmap.Width;
			Height = bitmap.Height;

			// Load the data from the bitmap
			var textureData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);

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
				UseBgra ? GLPixelFormat.Bgra : GLPixelFormat.Rgba,
				PixelType.UnsignedByte,
				textureData.Scan0);

			// Free the data since we won't need it anymore
			bitmap.UnlockBits(textureData);
			bitmap.Dispose();
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		/// <summary>
		///     Deletes the texture.
		/// </summary>
		public void Dispose()
		{
			GL.DeleteTexture(_texture);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///     Releases the texture without deleting it.
		/// </summary>
		public void Release()
		{
			GC.SuppressFinalize(this);
		}

		~Texture2D()
		{
			Dispose();
		}

		#endregion

		public void MapSubImage(IntPtr pixels)
		{
			GL.BindTexture(TextureTarget.Texture2D, _texture);
			GL.TexSubImage2D(
				TextureTarget.Texture2D, 0,
				0, 0, Width, Height,
				UseBgra ? GLPixelFormat.Bgra : GLPixelFormat.Rgba,
				PixelType.UnsignedByte, pixels);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void AttachToFramebuffer(int frameBuffer, FramebufferAttachment attachment)
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
			GL.FramebufferTexture2D(
				FramebufferTarget.Framebuffer, attachment,
				TextureTarget.Texture2D, _texture, 0);
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