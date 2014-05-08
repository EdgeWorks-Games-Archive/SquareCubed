using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SquareCubed.Common.Utils;

namespace SquareCubed.PluginLoader
{
	public class PluginLoader<TPlugin, TConstParam> : IDisposable where TPlugin : IDisposable
	{
		private readonly Logger _logger = new Logger("Plugins");

		#region Plugin Entries

		public Dictionary<string, PluginEntry> PluginTypes { get; private set; }
		public List<TPlugin> LoadedPlugins { get; set; }

		#endregion

		#region Initialization and Cleanup

		private bool _disposed;

		public PluginLoader()
		{
			_logger.LogInfo("Initializing plugin loader...");

			PluginTypes = new Dictionary<string, PluginEntry>();
			LoadedPlugins = new List<TPlugin>();

			_logger.LogInfo("Finished initializing plugin loader!");
		}

		~PluginLoader()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool managed)
		{
			// Prevent double disposing and disposing managed when told not to
			if (_disposed || !managed) return;
			_disposed = true;

			// Clean Up Plugins Here
			UnloadAllPlugins();
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
					.Where(type => type.GetInterface(typeof (TPlugin).FullName) != null)))
			{
				// Retrieve plugin info
				var attr = pluginType.GetCustomAttribute<PluginAttribute>();

				// Make sure it HAS plugin info
				if (attr == null)
				{
					_logger.LogInfo("Plugin {0} doesn't have a PluginAttribute, ignored!", pluginType.FullName);
					continue;
				}

				// Check if plugin ID already added
				PluginEntry entry;
				if (!PluginTypes.TryGetValue(attr.Id, out entry))
				{
					// Not found, add a new entry
					entry = new PluginEntry();
					PluginTypes.Add(attr.Id, entry);

					_logger.LogInfo("Added new plugin: " + attr.Name);
				}

				// Check if version already added
				if (entry.Versions.ContainsKey(attr.Version))
				{
					// We can't add the same plugin with the same version twice
					_logger.LogInfo("Version {0} of plugin {1} already added, ignored!", attr.Version.ToString(), attr.Name);
					continue;
				}

				// Add version
				entry.Versions.Add(attr.Version, pluginType);
				_logger.LogInfo("Added new version {0} of plugin {1}!", attr.Version.ToString(), attr.Name);
			}

			_logger.LogInfo("Finished detecting plugins!");
		}

		public void LoadAllPlugins(TConstParam param)
		{
			if (!TryLoadAllPlugins(param))
				throw new Exception("Multiple versions of one plugin detected!");
		}

		public bool TryLoadAllPlugins(TConstParam param)
		{
			foreach (var pluginType in PluginTypes.Values)
			{
				if (pluginType.Versions.Count != 1)
					return false;

				var plugin = (TPlugin) Activator.CreateInstance(pluginType.Versions.Values.First(), param);
				LoadedPlugins.Add(plugin);
			}
			return true;
		}

		public void LoadPlugin(string id, Version version, TConstParam param)
		{
			var plugin = (TPlugin) Activator.CreateInstance(PluginTypes[id].Versions[version], param);
			LoadedPlugins.Add(plugin);
		}

		public void UnloadAllPlugins()
		{
			LoadedPlugins.ForEach(p => p.Dispose());
			LoadedPlugins.Clear();
		}

		#endregion
	}
}