using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace SquareCubed.Client.Graphics
{
	public class Texture2D
	{
		private readonly uint _textureId;

		public Texture2D(string path)
		{
			// If the file doesn't exist, we can't do anything
			if (!File.Exists(path)) throw new Exception("Can't find file \"" + path + "\"!");

			// Load the data from the image file
			var textureBitmap = new Bitmap(path);
			var textureData = textureBitmap.LockBits(
				new Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format24bppRgb);

			// Generate and bind a new OpenGL texture
			GL.GenTextures(1, out _textureId);
			GL.BindTexture(TextureTarget.Texture2D, _textureId);

			// Configure the texture
			GL.TexEnv(TextureEnvTarget.TextureEnv,
				TextureEnvParameter.TextureEnvMode, (float) TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMinFilter, (float) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMagFilter, (float) TextureMagFilter.Linear);

			// Load the texture
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0, // level
				PixelInternalFormat.Three,
				textureBitmap.Width, textureBitmap.Height,
				0, // border
				OpenTK.Graphics.OpenGL.PixelFormat.Rgb,
				PixelType.UnsignedByte,
				textureData.Scan0);

			// Free the data since we won't need it anymore
			textureBitmap.UnlockBits(textureData);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void Render(Vector2 position, Vector2 size)
		{
			GL.BindTexture(TextureTarget.Texture2D, _textureId);

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

			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
	}
}