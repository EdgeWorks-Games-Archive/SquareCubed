using System;
using System.Diagnostics.Contracts;
using Coherent.UI;

namespace SquareCubed.Client.Gui
{
	public class Gui // TODO: Make IDisposable
	{
		private SystemSettings _settings;
		private UISystem _system;
		private EventListener _eventListener;

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

			IsLoaded = true;
		}

		public void Unload()
		{
			Contract.Requires<InvalidOperationException>(
				IsLoaded,
				"GUI needs to be loaded before it can be unloaded.");

			_system.Uninitialize();
			_system.Dispose(); // TODO: Dispose in our own dispose method instead
			_system = null;

			_settings = null;

			_eventListener.Dispose(); // TODO: Dispose in our own dispose method instead
			_eventListener = null;
		}
	}
}