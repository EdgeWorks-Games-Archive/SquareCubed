using OpenTK;
using SquareCubed.Client.Units;

namespace SquareCubed.Client.Structures.Objects
{
	public enum ProximityStatus
	{
		NotWithin,
		Within
	}

	/// <summary>
	///     Helper class to detect changes on a unit's proximity to an object.
	///     Can only handle a single unit at a time. Pass the unit you need to
	///     track to the Update function. Default state is not within proximity.
	/// </summary>
	public class UnitProximityHelper
	{
		private readonly ClientObject _obj;

		public UnitProximityHelper(ClientObject obj)
		{
			_obj = obj;
			Status = ProximityStatus.NotWithin;
		}

		public ProximityStatus Status { get; private set; }
		public float Range { get; set; }

		/// <summary>
		///     Updates the detection state, fires off events on changes.
		/// </summary>
		/// <param name="unit">
		///     The unit to update the detection state with.
		///     Null resets the state to default.
		/// </param>
		public void Update(Unit unit)
		{
			if (unit == null || (unit.Position - _obj.Position).LengthFast > Range)
				Status = ProximityStatus.NotWithin;
			else
				Status = ProximityStatus.Within;
		}
	}
}