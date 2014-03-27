using System;
using System.Diagnostics.Contracts;
using Coherent.UI;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Gui
{
	public class Gui : IDisposable
	{
		// Listeners
		private EventListener _eventListener;

		// Coherent UI System
		private SystemSettings _settings;
		private UISystem _system;

		// Graphics Resources
		private ShaderProgram _shader;

		public bool IsLoaded { get; private set; }

		public void Load()
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

			// Make sure it got set up correctly
			if (_system == null)
				throw new Exception("Failed to initialize CoherentUI!");

			_shader = new ShaderProgram(
				"Shaders/CoherentUI.vert",
				"Shaders/CoherentUI.frag");

			IsLoaded = true;
		}

		public void Unload()
		{
			Contract.Requires<InvalidOperationException>(
				IsLoaded,
				"GUI needs to be loaded before it can be unloaded.");

			// Clean up the Coherent UI system
			_system.Uninitialize();
			_system.Dispose(); // TODO: Dispose in our own dispose method instead
			_system = null;
			_settings = null;

			// Clean up all the Graphics Resources
			_shader.Dispose();
			_shader = null;

			// Clean up the Listeners
			_eventListener.Dispose(); // TODO: Dispose in our own dispose method instead
			_eventListener = null;

			IsLoaded = false;
		}

		public void Dispose()
		{
			// We only have managed resources to dispose of
			if (IsLoaded) Unload();
		}
	}
}