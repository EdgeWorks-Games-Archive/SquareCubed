using System;

namespace SquareCubed.PluginLoader
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PluginAttribute : Attribute
	{
		public string Id { get; private set; }
		public string Name { get; private set; }
		public Version Version { get; private set; }

		public PluginAttribute(string id, string name, int versionMajor, int versionMinor)
		{
			Id = id;
			Name = name;
			Version = new Version(versionMajor, versionMinor);
		}
	}
}
