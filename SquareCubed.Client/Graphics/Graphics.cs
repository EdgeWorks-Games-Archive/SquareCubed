using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace SquareCubed.Client.Graphics
{
	public sealed class Graphics
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
			_upscaledSize = new Size(Camera.Resolution.Width * 2, Camera.Resolution.Height * 2);
			var maxSize = GL.GetInteger(GetPName.MaxTextureSize);
			if (_upscaledSize.Width > maxSize || _upscaledSize.Height > maxSize)
				throw new InvalidOperationException("GPU maximum texture size is not big enough to support supersampling.");

			// Generate the upscaled background texture
			var texture = new Texture2D(_upscaledSize);

			// Background frame buffer and attach the texture to it
			_usFrameBuffer = GL.GenFramebuffer();
			texture.AttachToFramebuffer(_usFrameBuffer, FramebufferAttachment.ColorAttachment0);

			// Clean up
			GL.BindTexture(TextureTarget.Texture2D, 0);
			texture.Release();
		}

		#endregion

		#region Game Loop

		public void BeginSceneRender()
		{
			// Ensure OpenGL settings are set correctly
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			// Initialize Camera
			Camera.SetMatrices();

			// Set framebuffer to the upscaled one
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, _usFrameBuffer);
			GL.Viewport(0, 0, _upscaledSize.Width, _upscaledSize.Height);
		}

		public void EndSceneRender()
		{
			// Set the framebuffers (read = upscaled, write = default)
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _usFrameBuffer);
			GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

			// Downsample the upscaled buffer into the default buffer TODO: Do with VBO triangle instead
			GL.BlitFramebuffer(
				0, 0, _upscaledSize.Width, _upscaledSize.Height,
				0, 0, Camera.Resolution.Width, Camera.Resolution.Height,
				ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);
		}

		public void SwapBuffers()
		{
			_window.SwapBuffers();
		}

		#endregion
	}
}