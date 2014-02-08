using System;

namespace SquareCubed.PluginLoader
{
    public class PluginLoader<TPlugin> : IDisposable
	{
		#region Initialization and Cleanup

		private bool _disposed;

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
	}
}
