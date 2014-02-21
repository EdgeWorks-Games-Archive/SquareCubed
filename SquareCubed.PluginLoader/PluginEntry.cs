using System;
using System.Collections.Generic;

namespace SquareCubed.PluginLoader
{
	public class PluginEntry
	{
		public PluginEntry()
		{
			Versions = new Dictionary<Version, Type>();
		}

		public Dictionary<Version, Type> Versions { get; private set; }
	}
}
