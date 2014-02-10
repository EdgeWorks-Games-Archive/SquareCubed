using System;
using System.IO;
using System.Reflection;
using System.Linq;
using SquareCubed.Utils.Logging;

namespace SquareCubed.PluginLoader
{
    public class PluginLoader<TPlugin> : IDisposable
	{
		private readonly Logger _logger;

		#region Initialization and Cleanup

		private bool _disposed;

        public PluginLoader()
        {
			_logger = new Logger("Plugins");
		}

		public virtual void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Prevent Double Disposing
			if (_disposed) return;

			if (disposing)
			{
				// Clean Up Plugins Here
			}

			_disposed = true;
		}

		#endregion

		#region Utility Functions

		public virtual void DetectPlugins()
		{
			_logger.LogInfo("Detecting plugins...");

			// Make sure Plugin Directory Exists
			if (!Directory.Exists("Plugins")) throw new Exception("Can't find plugins directory!");

			// Detect all DLL Files (Plugins are compiled to DLL class libraries)
			var dllFiles = Directory.GetFiles("Plugins", "*.dll", SearchOption.AllDirectories);

			// Iterate through each plugin file
			foreach (var pluginType in dllFiles
				// Load Assemblies from DLL Files
				.Select(dllFile => Assembly.Load(AssemblyName.GetAssemblyName(dllFile)))
				// Ignore Assemblies that Couldn't be Loaded
				.Where(assembly => assembly != null)
				.SelectMany(assembly => assembly.GetTypes()
					// Filter out interfaces and abstract classes
					.Where(type => !type.IsInterface && !type.IsAbstract)
					// Filter by classes that implement the plugin interface
					.Where(type => type.GetInterface(typeof(TPlugin).FullName) != null)))
			{
				_logger.LogInfo("Found: " + pluginType.FullName);
			}

			_logger.LogInfo("Finished detecting plugins!");
		}

		#endregion
	}
}
