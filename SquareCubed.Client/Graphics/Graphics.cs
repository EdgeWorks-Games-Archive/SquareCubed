using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace SquareCubed.Client.Graphics
{
	public class Graphics
	{
		private readonly IGameWindow _window;

		public Camera Camera { get; private set; }

		#region Initialization and Cleanup

		public Graphics(IGameWindow window)
		{
			Contract.Requires<ArgumentNullException>(window != null);

			_window = window;
			Camera = new Camera(_window.Size);
		}

		#endregion

		#region Game Loop

		public virtual void BeginRender()
		{
			// Ensure settings are set correctly
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Multisample);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);

			// Initialize Camera
			Camera.SetMatrices();
		}

		public virtual void EndRender()
		{
			_window.SwapBuffers();
		}

		#endregion
	}
}