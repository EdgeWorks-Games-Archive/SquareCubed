using SquareCubed.Common.Utils;
using SquareCubed.PluginLoader;

namespace SQCore.Common
{
	[Plugin(
		"Blink.Core",
		"Blink",
		"0.1")
	]
    public abstract class CommonPlugin
    {
		protected readonly Logger Logger = new Logger("Blink");
    }
}