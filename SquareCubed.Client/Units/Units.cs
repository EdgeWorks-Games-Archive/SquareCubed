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
		private readonly Structures.Structures _structures;

		public Units(Client client, Structures.Structures structures)
		{
			_structures = structures;
			_network = new UnitsNetwork(client, this);
		}

		#region Network Callbacks

		internal void OnUnitPhysics(int id, Vector2 position)
		{
			// Try to get the unit, if we can't just ignore it
			Unit unit;
			if (!_units.TryGetValue(id, out unit)) return;

			// Set the data
			unit.ProcessPhysicsPacketData(position);
		}

		internal void OnUnitData(Unit unit)
		{
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

		internal void OnUnitTeleport(int id, Vector2 position, int structure)
		{
			// Try to get the unit, if we can't just ignore it
			Unit unit;
			if (!_units.TryGetValue(id, out unit)) return;

			// Set the data
			unit.Position = position;
			unit.Structure = _structures.GetOrNull(structure);
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