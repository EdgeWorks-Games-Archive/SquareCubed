using System;

namespace SquareCubed.Client
{
	public class TickEventArgs : EventArgs
	{
		/// <summary>
		///     The time elapsed between the start of the last
		///     frame and the start of the current frame.
		/// </summary>
		public float ElapsedTime { get; set; }
	}
}