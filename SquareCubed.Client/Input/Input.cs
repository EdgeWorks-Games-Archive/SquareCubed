using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Input;
using SquareCubed.Client.Graphics;
using SquareCubed.Client.Window;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Input
{
	public class Input
	{
		private readonly Dictionary<Key, bool> _keys = new Dictionary<Key, bool>();
		private readonly Camera _camera;

		public Input(IExtGameWindow window, Camera camera)
		{
			Debug.Assert(window != null);
			Debug.Assert(camera != null);

			_camera = camera;

			// Bind keyboard events
			window.KeyDown += OnKeyDown;
			window.KeyUp += OnKeyUp;

			// Bind mouse events
			window.MouseMove += OnMouseMove;

			// Add a axis keys to be tracked
			TrackKey(Key.W);
			TrackKey(Key.A);
			TrackKey(Key.S);
			TrackKey(Key.D);
		}

		#region Input Event Handlers

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

		private void OnMouseMove(object s, MouseMoveEventArgs e)
		{
			var absolute = new Vector2i(e.X, e.Y);
			MouseState = new MouseState
			{
				AbsolutePosition = absolute,
				RelativePosition = _camera.AbsoluteToRelative(absolute)
			};
		}

		#endregion

		#region Public Helpers

		public Vector2 Axes { get; private set; }
		public MouseState MouseState { get; private set; }

		public void TrackKey(Key key)
		{
			_keys[key] = false;
		}

		public bool GetKey(Key key)
		{
			return _keys[key];
		}

		public void UpdateAxes()
		{
			// Translate the pressed keys to axes
			var newAxes = new Vector2();
			if (GetKey(Key.D)) newAxes.X += 1;
			if (GetKey(Key.A)) newAxes.X -= 1;
			if (GetKey(Key.W)) newAxes.Y += 1;
			if (GetKey(Key.S)) newAxes.Y -= 1;

			// Normalize the axes for easy usage and update
			newAxes.NormalizeFast();
			Axes = newAxes;
		}

		#endregion
	}
}