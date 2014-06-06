using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SquareCubed.Client.Gui.Controls;
using SquareCubed.Client.Window;

namespace SquareCubed.Client.Gui
{
	public class Gui : GuiControl.GuiParentControl
	{
		private Point _previousMousePos;
		private MousePressData.MousePressEndEvent _previousMousePressEvent;
		private Size _size;

		internal Gui(IExtGameWindow gameWindow)
		{
			_size = gameWindow.ClientSize;

			gameWindow.KeyPress += OnKeyPressEvent;

			gameWindow.MouseMove += OnMouseMoveEvent;
			gameWindow.MouseDown += OnMouseDownEvent;
			gameWindow.MouseUp += OnMouseUpEvent;
		}

		public override Size Size
		{
			get { return _size; }
			set { _size = value; }
		}

		public override Size InternalOffset
		{
			get { return new Size(0, 0); }
		}

		private void OnKeyPressEvent(object sender, KeyPressEventArgs e)
		{
			HandleKeyChar(e.KeyChar);
		}

		private void OnMouseDownEvent(object sender, MouseButtonEventArgs e)
		{
			_previousMousePressEvent = new MousePressData.MousePressEndEvent();
			HandleMouseDown(new MousePressData(e.Position, e.Button, _previousMousePressEvent));
		}

		private void OnMouseUpEvent(object sender, MouseButtonEventArgs e)
		{
			HandleMouseUp(new MousePressData(e.Position, e.Button, _previousMousePressEvent));
			_previousMousePressEvent.Invoke();
		}

		private void OnMouseMoveEvent(object sender, MouseMoveEventArgs e)
		{
			HandleMouseMove(new MouseMoveData(e.Position, _previousMousePos));
			_previousMousePos = e.Position;
		}

		internal override void Render(float delta)
		{
			// Set framebuffer to the default one
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.Viewport(0, 0, _size.Width, _size.Height);

			// Reset the matrices to default values
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, _size.Width, _size.Height, 0, 1, -1);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			base.Render(delta);
		}
	}
}