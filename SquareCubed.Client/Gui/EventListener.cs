using Coherent.UI;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Gui
{
	class EventListener : Coherent.UI.EventListener
	{
		private readonly Logger _logger = new Logger("GUI");

		public bool IsSystemReady { get; private set; }

		public EventListener()
		{
			_logger.LogInfo("Initialized listener!");
		}

		public override void SystemReady()
		{
			_logger.LogInfo("System ready!");
			IsSystemReady = true;
		}

		public override void OnError(SystemError arg0)
		{
			_logger.LogInfo("An error occured! {0} (#{1})", arg0.Error, arg0.ErrorCode);
		}
	}
}
