using SquareCubed.PluginLoader;
using SquareCubed.Utils.Logging;

namespace SQCore.Common
{
	[Plugin(
		"Blink.Core",
		"Blink Core",
		"0.1")
	]
    public abstract class CommonPlugin
    {
		protected readonly Logger Logger = new Logger("Blink");
    }
}