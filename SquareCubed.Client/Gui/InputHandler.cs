using Coherent.UI;
using OpenTK;
using OpenTK.Input;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Gui
{
	/// <summary>
	///     Bridges OpenTK and CoherentUI input systems.
	/// </summary>
	internal class InputHandler
	{
		private readonly INativeWindow _window;

		private ViewListener _viewListener;

		public InputHandler(INativeWindow window)
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
					_window.KeyPress += window_KeyPress;
					_window.KeyDown += _window_KeyDown;
					_window.KeyUp += window_KeyUp;
				}
				else if (_viewListener != null && value == null)
				{
					_window.KeyPress -= window_KeyPress;
					_window.KeyDown -= _window_KeyDown;
					_window.KeyUp -= window_KeyUp;
				}

				// Update value
				_viewListener = value;
			}
		}

		private void window_KeyPress(object sender, KeyPressEventArgs e)
		{
			var eventData = new KeyEventData
			{
				//Modifiers = GetEventModifiersState(),
				KeyCode = e.KeyChar, // Not sure this is the right one
				IsNumPad = false, // Indeterminate
				IsAutoRepeat = false, // Indeterminate
				Type = KeyEventData.EventType.Char
			};
			_viewListener.View.KeyEvent(eventData);
		}

		private void _window_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			var eventData = new KeyEventData
			{
				//Modifiers = GetEventModifiersState(),
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
				//Modifiers = GetEventModifiersState(),
				KeyCode = e.Key.ToVkCode(),
				IsNumPad = false, // Indeterminate
				IsAutoRepeat = false, // Indeterminate
				Type = KeyEventData.EventType.KeyUp
			};
			_viewListener.View.KeyEvent(eventData);
		}
	}
}