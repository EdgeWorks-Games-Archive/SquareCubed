using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace SquareCubed.Client.Graphics
{
	public class Graphics
	{
		private readonly IGameWindow _window;
		private readonly Size _upscaledSize;
		private readonly int _usFrameBuffer;

		public Camera Camera { get; private set; }

		#region Initialization and Cleanup

		public Graphics(IGameWindow window)
		{
			Contract.Requires<ArgumentNullException>(window != null);
			
			_window = window;
			Camera = new Camera(_window.ClientSize);

			// Make sure the required texture size doesn't exceed limits
			_upscaledSize = new Size(_window.Width * 2, _window.Height * 2);
			var maxSize = GL.GetInteger(GetPName.MaxTextureSize);
			Console.WriteLine("Test: " + maxSize);
			if (_upscaledSize.Width > maxSize || _upscaledSize.Height > maxSize)
				throw new InvalidOperationException("GPU maximum texture size is not big enough to support supersampling.");

			// Generate the upscaled background texture
			var usTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, usTexture);

			// Allocate storage for the texture
			GL.TexImage2D(
				TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
				_upscaledSize.Width, _upscaledSize.Height,
				0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);

			// Generate the upscaled background frame buffer
			_usFrameBuffer = GL.GenFramebuffer();
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, _usFrameBuffer);

			// Attach the texture to the frame buffer color attachment 0
			GL.FramebufferTexture2D(
				FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
				TextureTarget.Texture2D, usTexture, 0);

			// Clean up
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		#endregion

		#region Game Loop

		public void BeginRender()
		{
			// Ensure settings are set correctly
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Multisample);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);

			// Initialize Camera
			Camera.SetMatrices();

			// Set framebuffer to the upscaled one
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, _usFrameBuffer);
			GL.Viewport(0, 0, _upscaledSize.Width, _upscaledSize.Height);
		}

		public void EndRender()
		{
			// Set the framebuffers (read = upscaled, write = default)
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _usFrameBuffer);
			GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

			// Downsample the upscaled buffer into the default buffer TODO: Do with VBO triangle instead
			GL.BlitFramebuffer(
				0, 0, _upscaledSize.Width, _upscaledSize.Height,
				0, 0, _window.Width, _window.Height,
				ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);
		}

		public void BeginRenderGui()
		{
			// Set framebuffer to the default one
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.Viewport(0, 0, _window.Width,_window.Height);

			// Reset the matrices to default values
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
		}

		public void EndRenderAll()
		{
			_window.SwapBuffers();
		}

		#endregion
	}
}