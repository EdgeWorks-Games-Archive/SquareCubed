using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Window
{
	/// <summary>
	///     Representation of the game window.
	///     Not unit tested because we don't want unit tests to set up an actual window.
	/// </summary>
	public class Window : GameWindow
	{
		public Window()
			: base(1024, 768, new GraphicsMode(32, 0, 0, 4), "SquareCubed Engine")
		{
		}

		#region Event Callbacks

		protected override void OnLoad(EventArgs e)
		{
			TargetRenderFrequency = 60;
			TargetUpdateFrequency = 60;
			VSync = VSyncMode.Adaptive;
			WindowBorder = WindowBorder.Fixed;

			base.OnLoad(e);
		}

		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(0, 0, Width, Height);

			base.OnResize(e);
		}

		#endregion
	}
}