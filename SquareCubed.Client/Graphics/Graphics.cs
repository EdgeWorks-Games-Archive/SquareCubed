using System;
using System.Diagnostics.Contracts;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace SquareCubed.Client.Graphics
{
	public class Graphics
	{
		private readonly IGameWindow _window;
		private readonly float _backBufferScale;
		private readonly int _usFrameBuffer;

		public Camera Camera { get; private set; }

		#region Initialization and Cleanup

		public Graphics(IGameWindow window, float backBufferScale = 2.0f)
		{
			Contract.Requires<ArgumentNullException>(window != null);
			Contract.Requires<InvalidOperationException>(backBufferScale <= 2.0f);
			
			_window = window;
			Camera = new Camera(_window.ClientSize);

			_backBufferScale = backBufferScale;
			var maxSize = GL.GetInteger(GetPName.MaxTextureSize);
			if ((int)(_window.Width * _backBufferScale) > maxSize || (int)(_window.Height * _backBufferScale) > maxSize)
				throw new ArgumentOutOfRangeException("backBufferScale", "Back buffer scale results in higher size textures than allowed.");

			// Generate the upscaled background texture
			var usTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, usTexture);

			// Allocate storage for the texture
			GL.TexImage2D(
				TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
				(int)(_window.Width * _backBufferScale), (int)(_window.Height * _backBufferScale),
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
			GL.Viewport(0, 0, (int)(_window.Width * _backBufferScale), (int)(_window.Height * _backBufferScale));
		}

		public void EndRender()
		{
			// Set the framebuffers (read = upscaled, write = default)
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _usFrameBuffer);
			GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

			// Downsample the upscaled buffer into the default buffer TODO: Do with VBO triangle instead
			GL.BlitFramebuffer(
				0, 0, (int)(_window.Width * _backBufferScale), (int)(_window.Height * _backBufferScale),
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