using System;
using System.Diagnostics.Contracts;
using Coherent.UI;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Gui
{
	public sealed class Gui : IDisposable
	{
		#region Coherent UI Resources

		private EventListener _eventListener;
		private SystemSettings _settings;
		private UISystem _system;

		#endregion

		#region Graphics Resources

		private ShaderProgram _shader;
		private Texture2D _texture;

		#endregion

		public bool IsLoaded { get; private set; }

		public void Dispose()
		{
			// We only have managed resources to dispose of
			if (IsLoaded) Unload();
		}

		public void Load(int width, int height)
		{
			Contract.Requires<InvalidOperationException>(
				!IsLoaded,
				"GUI is already loaded.");

			// Set up Event Listener
			_eventListener = new EventListener();

			// Configure CoherentUI
			_settings = new SystemSettings
			{
#if DEBUG
				DebuggerPort = 1234
#endif
			};

			// Set up the UI System
			_system = CoherentUI_Native.InitializeUISystem(
				Versioning.SDKVersion, License.COHERENT_KEY,
				_settings, _eventListener,
				Severity.Warning, null, null);

			// Make sure it got created
			if (_system == null)
				throw new Exception("Failed to initialize CoherentUI!");

			// Create a shader program for Coherent UI
			_shader = new ShaderProgram(
				"Shaders/CoherentUI.vert",
				"Shaders/CoherentUI.frag");

			// Create a texture for it as well
			// The texture needs to have alpha activated since Coherent UI will need it
			_texture = new Texture2D(width, height, true);

			IsLoaded = true;
		}

		public void Unload()
		{
			Contract.Requires<InvalidOperationException>(
				IsLoaded,
				"GUI needs to be loaded before it can be unloaded.");

			// Clean up the Coherent UI system
			_system.Uninitialize();
			_system.Dispose();
			_system = null;
			_settings = null;

			// Clean up all the Graphics Resources
			_shader.Dispose();
			_shader = null;

			// Clean up the Listeners
			_eventListener.Dispose();
			_eventListener = null;

			// Clean up the Graphics Resources
			// TODO: _texture.Dispose() needs to be created
			_texture = null;

			IsLoaded = false;
		}
	}
}