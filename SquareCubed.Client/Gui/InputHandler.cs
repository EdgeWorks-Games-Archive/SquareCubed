using System;
using System.Windows.Forms;
using Coherent.UI;
using OpenTK.Input;
using SquareCubed.Client.Window;
using SquareCubed.Common.Data;
using KeyPressEventArgs = OpenTK.KeyPressEventArgs;

namespace SquareCubed.Client.Gui
{
	/// <summary>
	///     Bridges OpenTK and CoherentUI input systems.
	/// </summary>
	internal class InputHandler
	{
		private readonly IExtGameWindow _window;

		private ViewListener _viewListener;

		public InputHandler(IExtGameWindow window)
		{
			_window = window;
		}

		public ViewListener ViewListener
		{
			set
			{
				// Update event hooks if needed
				if (_viewListener == null && value != null)
				{
					value.ViewCreated += viewListener_ViewCreated;
				}
				else if (_viewListener != null && value == null)
				{
					_viewListener.ViewCreated -= viewListener_ViewCreated;

					_window.KeyPress -= window_KeyPress;
					_window.KeyDown -= window_KeyDown;
					_window.KeyUp -= window_KeyUp;

					_window.MouseDown -= window_MouseDown;
					_window.MouseUp -= window_MouseUp;
					_window.MouseMove -= window_MouseMove;
				}

				// Update value
				_viewListener = value;
			}
		}

		void viewListener_ViewCreated(Coherent.UI.View view)
		{
			_window.KeyPress += window_KeyPress;
			_window.KeyDown += window_KeyDown;
			_window.KeyUp += window_KeyUp;

			_window.MouseDown += window_MouseDown;
			_window.MouseUp += window_MouseUp;
			_window.MouseMove += window_MouseMove;
		}


		#region Keyboard

		private void window_KeyPress(object sender, KeyPressEventArgs e)
		{
			var eventData = new KeyEventData
			{
				Modifiers = GetEventModifiersState(),
				KeyCode = e.KeyChar, // Not sure this is the right one
				IsNumPad = false, // Indeterminate
				IsAutoRepeat = false, // Indeterminate
				Type = KeyEventData.EventType.Char
			};
			_viewListener.View.KeyEvent(eventData);
		}

		private void window_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			var eventData = new KeyEventData
			{
				Modifiers = GetEventModifiersState(),
				KeyCode = e.Key.ToVkCode(),
				IsNumPad = false, // Indeterminate
				IsAutoRepeat = false, // Indeterminate
				Type = KeyEventData.EventType.KeyDown
			};
			_viewListener.View.KeyEvent(eventData);
		}

		private void window_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			var eventData = new KeyEventData
			{
				Modifiers = GetEventModifiersState(),
				KeyCode = e.Key.ToVkCode(),
				IsNumPad = false, // Indeterminate
				IsAutoRepeat = false, // Indeterminate
				Type = KeyEventData.EventType.KeyUp
			};
			_viewListener.View.KeyEvent(eventData);
		}

		private static EventModifiersState GetEventModifiersState()
		{
			var keys = Control.ModifierKeys;

			var state = new EventModifiersState
			{
				IsCtrlDown = keys.HasFlag(Keys.Control),
				IsAltDown = keys.HasFlag(Keys.Alt),
				IsShiftDown = keys.HasFlag(Keys.Shift),
				IsCapsOn = keys.HasFlag(Keys.CapsLock),
				IsNumLockOn = keys.HasFlag(Keys.NumLock)
			};

			return state;
		}

		#endregion

		#region Mouse

		void window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			UpdateMouseEventData(e);
			_mouseEventData.Type = MouseEventData.EventType.MouseDown;
			_viewListener.View.MouseEvent(_mouseEventData);
		}

		void window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			UpdateMouseEventData(e);
			_mouseEventData.Type = MouseEventData.EventType.MouseUp;
			_viewListener.View.MouseEvent(_mouseEventData);
		}

		private void window_MouseMove(object sender, MouseMoveEventArgs e)
		{
			_mouseEventData.X = e.X;
			_mouseEventData.Y = e.Y;
			_mouseEventData.Type = MouseEventData.EventType.MouseMove;
			_viewListener.View.MouseEvent(_mouseEventData);
		}

		private readonly MouseEventData _mouseEventData = new MouseEventData();

		private void UpdateMouseEventData(MouseButtonEventArgs e)
		{
			// Change the mouse modifiers into a form CoherentUI can work with
			var mouseMods = new EventMouseModifiersState
			{
				IsLeftButtonDown = e.Button.HasFlag(MouseButton.Left),
				IsMiddleButtonDown = e.Button.HasFlag(MouseButton.Middle),
				IsRightButtonDown = e.Button.HasFlag(MouseButton.Right)
			};

			_mouseEventData.Modifiers = GetEventModifiersState();
			_mouseEventData.MouseModifiers = mouseMods;

			_mouseEventData.X = e.X;
			_mouseEventData.Y = e.Y;

			_mouseEventData.Button = MouseEventData.MouseButton.ButtonNone;
			if (e.Button.HasFlag(MouseButton.Left))
				_mouseEventData.Button = MouseEventData.MouseButton.ButtonLeft;
			else if (e.Button.HasFlag(MouseButton.Middle))
				_mouseEventData.Button = MouseEventData.MouseButton.ButtonMiddle;
			else if (e.Button.HasFlag(MouseButton.Right))
				_mouseEventData.Button = MouseEventData.MouseButton.ButtonRight;
		}

		#endregion
	}
}