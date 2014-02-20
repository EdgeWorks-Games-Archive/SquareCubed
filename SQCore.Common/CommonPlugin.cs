using SquareCubed.PluginLoader;
using SquareCubed.Utils.Logging;

namespace SQCore.Common
{
	[Plugin(
		"SquareCubed.Core",
		"SquareCubed Core",
		0, 1)
	]
    public class CommonPlugin
    {
		protected readonly Logger Logger = new Logger("SQCore");
    }
}