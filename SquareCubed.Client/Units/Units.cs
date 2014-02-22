using System;
using System.Collections.Generic;
using OpenTK;

namespace SquareCubed.Client.Units
{
	public class Units
	{
		private readonly UnitsNetwork _network;
		private readonly Dictionary<uint, Unit> _units = new Dictionary<uint, Unit>();

		public Units(Client client)
		{
			_network = new UnitsNetwork(client, this);
		}

		#region Network Callbacks

		public void OnUnitPhysics(uint key, Vector2 position)
		{
			Unit unit;

			// Try to get the unit, if we can't just ignore it
			if (!_units.TryGetValue(key, out unit)) return;

			// Set the data
			unit.Position = position;
		}

		public void OnUnitData(uint key)
		{
			Unit unit;

			// Try to get the unit, if we can't we need to add it
			if (!_units.TryGetValue(key, out unit))
			{
				unit = new Unit(key);
				_units.Add(key, unit);
			}

			// Set data here once we got data to set
		}

		#endregion
	}
}