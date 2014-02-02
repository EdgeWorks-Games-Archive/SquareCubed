using System;

namespace SquareCubed.Server
{
	public class Server : IDisposable
	{
		#region Initialization and Cleanup

		private bool _disposed;

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Prevent Double Disposing
			if (_disposed) return;

			if (disposing)
			{
				/* Nothing to do */
			}

			_disposed = true;
		}

		#endregion

		public void Run()
		{
		}
	}
}