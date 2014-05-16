using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using OpenTK;
using SquareCubed.Client.Structures;

namespace SquareCubed.Client.Units
{
	public class Units
	{
		private readonly UnitsNetwork _network;
		private readonly UnitsRenderer _renderer = new UnitsRenderer();
		private readonly Dictionary<int, Unit> _units = new Dictionary<int, Unit>();

		public Units(Client client)
		{
			_network = new UnitsNetwork(client, this);
		}

		#region Network Callbacks

		public void OnUnitPhysics(int key, Vector2 position)
		{
			Unit unit;

			// Try to get the unit, if we can't just ignore it
			if (!_units.TryGetValue(key, out unit)) return;

			// Set the data
			unit.ProcessPhysicsPacketData(position);
		}

		public void OnUnitData(Unit unit)
		{
			Contract.Requires<ArgumentNullException>(unit != null);

			// Try to get the unit, if we can't we need to add it, otherwise overwrite it
			if (!_units.ContainsKey(unit.Id))
				_units.Add(unit.Id, unit);
			else
			{
				// Update the unit manually, we can't replace it because
				// there's derived types of Unit for player units.
				var oldUnit = _units[unit.Id];
				oldUnit.Position = unit.Position;
				oldUnit.Structure = unit.Structure;
				
				// Unlink the newly created unit from the structure so it gets garbage collected
				// TODO: This seriously needs improvements in how this works...
				unit.Structure = null;
			}
		}

		#endregion

		#region Utilitiy Functions

		public void Add(Unit unit)
		{
			Contract.Requires<ArgumentNullException>(unit != null);

			_units.Add(unit.Id, unit);
		}

		public Unit GetAndRemove(int key)
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

		public void RenderFor(ClientStructure structure)
		{
			Contract.Requires<ArgumentNullException>(structure != null);

			_renderer.RenderUnits(structure.Units);
		}

		#endregion
	}
}