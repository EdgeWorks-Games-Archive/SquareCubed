using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using OpenTK;
using OpenTK.Input;

namespace SquareCubed.Client.Input
{
	public class Input
	{
		private readonly Dictionary<Key, bool> _keys = new Dictionary<Key, bool>();

		public Input(Window.Window window)
		{
			Contract.Requires<ArgumentNullException>(window != null);

			// Bind keyboard events
			window.KeyDown += OnKeyDown;
			window.KeyUp += OnKeyUp;

			// Add a axis keys to be tracked
			_keys[Key.W] = false;
			_keys[Key.A] = false;
			_keys[Key.S] = false;
			_keys[Key.D] = false;
		}

		private void OnKeyDown(object s, KeyboardKeyEventArgs e)
		{
			// Check if the key is being tracked and if yes update it
			if (_keys.ContainsKey(e.Key))
				_keys[e.Key] = true;
		}

		private void OnKeyUp(object s, KeyboardKeyEventArgs e)
		{
			// Check if the key is being tracked and if yes update it
			if (_keys.ContainsKey(e.Key))
				_keys[e.Key] = false;
		}

		#region Input Direction Axes

		public Vector2 Axes { get; private set; }

		public void UpdateAxes()
		{
			// Translate the pressed keys to axes
			var newAxes = new Vector2();
			if (_keys[Key.D]) newAxes.X += 1;
			if (_keys[Key.A]) newAxes.X -= 1;
			if (_keys[Key.W]) newAxes.Y += 1;
			if (_keys[Key.S]) newAxes.Y -= 1;

			// Normalize the axes for easy usage and update
			newAxes.NormalizeFast();
			Axes = newAxes;
		}

		#endregion
	}
}