using System;

namespace SquareCubed.Network
{
    public class Network : IDisposable
	{
		#region Initialization and Cleanup

		private bool _disposed;

		public virtual void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Prevent double disposing and don't dispose if we're told not to
			if (_disposed || !disposing) return;
			_disposed = true;

			// Dispose stuff here
		}

		#endregion
	}
}
