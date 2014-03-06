using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace SquareCubed.Client.Units
{
	public class Units
	{
		private readonly UnitsNetwork _network;
		private readonly UnitsRenderer _renderer = new UnitsRenderer();
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
			unit.ProcessPhysicsPacketData(position);
		}

		public void OnUnitData(Unit unit)
		{
			Unit oldUnit;

			// Try to get the unit, if we can't we need to add it, otherwise overwrite it
			if (!_units.TryGetValue(unit.Id, out oldUnit))
				_units.Add(unit.Id, unit);
			else
				_units[unit.Id] = unit;
		}

		#endregion

		#region Utilitiy Functions

		public void Add(Unit unit)
		{
			_units.Add(unit.Id, unit);
		}

		public Unit GetAndRemove(uint key)
		{
			Unit unit;

			// Try to get the unit, if we can't we throw an exception
			if (!_units.TryGetValue(key, out unit))
				throw new Exception("Cannot remove because it doesn't exist.");

			// Remove and return
			_units.Remove(key);
			return unit;
		}

		#endregion

		#region Game Loop

		public void Render()
		{
			_renderer.RenderUnits(_units.Select(u => u.Value));
		}

		#endregion
	}
}