using System;
using SquareCubed.Client.Units;

namespace SquareCubed.Client.Structures.Objects
{
	public enum ProximityStatus
	{
		NotWithin,
		Within
	}

	public class ProximityEventArgs : EventArgs
	{
		/// <summary>
		///     The time elapsed between the start of the last
		///     frame and the start of the current frame.
		/// </summary>
		public ProximityStatus NewStatus { get; set; }
	}

	/// <summary>
	///     Helper class to detect changes on a unit's proximity to an object.
	///     Can only handle a single unit at a time. Pass the unit you need to
	///     track to the Update function. Default state is not within proximity.
	/// </summary>
	public class UnitProximityHelper
	{
		private readonly IClientObject _obj;

		public UnitProximityHelper(IClientObject obj, float range = 1.0f)
		{
			_obj = obj;
			Status = ProximityStatus.NotWithin;
			Range = range;
		}

		public ProximityStatus Status { get; private set; }
		public float Range { get; set; }
		public event EventHandler<ProximityEventArgs> Change;

		/// <summary>
		///     Updates the detection state, fires off events on changes.
		/// </summary>
		/// <param name="unit">
		///     The unit to update the detection state with.
		///     Null resets the state to default.
		/// </param>
		public void Update(Unit unit)
		{
			// Find the new status
			var nStatus = ProximityStatus.Within;
			if (unit == null || (unit.Position - _obj.Position).LengthFast > Range)
				nStatus = ProximityStatus.NotWithin;

			// If not different, don't do anything
			if (nStatus == Status) return;

			// Set it and fire an event
			Status = nStatus;
			if (Change != null) Change(this, new ProximityEventArgs {NewStatus = nStatus});
		}
	}
}