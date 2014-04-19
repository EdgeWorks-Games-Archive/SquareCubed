using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace SquareCubed.Client.Window
{
	/// <summary>
	///     Representation of the game window.
	///     Not unit tested because we don't want unit tests to set up an actual window.
	/// </summary>
	public class Window : GameWindow, IExtGameWindow
	{
		public Window()
			: base(1280, 720, new GraphicsMode(32, 0, 0, 16), "SquareCubed Engine")
		{
			Mouse.Move += (o, p) => MouseMove.Invoke(o, p);
			Mouse.ButtonDown += (o, p) => MouseDown.Invoke(o, p);
			Mouse.ButtonUp += (o, p) => MouseUp.Invoke(o, p);
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

		public event EventHandler<MouseMoveEventArgs> MouseMove = (o, p) => { };
		public event EventHandler<MouseButtonEventArgs> MouseDown = (o, p) => { };
		public event EventHandler<MouseButtonEventArgs> MouseUp = (o, p) => { };
	}
}