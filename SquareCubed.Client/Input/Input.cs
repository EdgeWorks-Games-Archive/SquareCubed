using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace SquareCubed.Client.Input
{
	public class Input
	{
		private readonly Dictionary<Key, bool> _keys = new Dictionary<Key, bool>();

		public Input(Window.Window window)
		{
			window.KeyDown += (s, e) => OnKeyChange(e, true);
			window.KeyUp += (s, e) => OnKeyChange(e, false);

			// Add a axis keys to be tracked
			_keys[Key.W] = false;
			_keys[Key.A] = false;
			_keys[Key.S] = false;
			_keys[Key.D] = false;
		}

		private void OnKeyChange(KeyboardKeyEventArgs e, bool down)
		{
			// Check if the key is being tracked and if yes update it
			if (_keys.ContainsKey(e.Key))
				_keys[e.Key] = down;
		}

		#region Input Direction Axes

		public Vector2 Axes { get; private set; }

		public void UpdateAxes()
		{
			var newAxes = new Vector2();
			if (_keys[Key.D]) newAxes.X += 1;
			if (_keys[Key.A]) newAxes.X -= 1;
			if (_keys[Key.W]) newAxes.Y += 1;
			if (_keys[Key.S]) newAxes.Y -= 1;
			newAxes.NormalizeFast();
			Axes = newAxes;
		}

		#endregion
	}
}