using System;
using System.Diagnostics;
using SquareCubed.Common.Utils;
using SquareCubed.Server.Players;

namespace SquareCubed.Server.Units
{
	public class Units
	{
		private readonly AutoDictionary<Unit> _units = new AutoDictionary<Unit>();
		private readonly UnitsNetwork _network;

		public Units(Network.Network network)
		{
			Debug.Assert(network != null);

			_network = new UnitsNetwork(network);
		}

		public void Add(Unit unit)
		{
			Debug.Assert(unit != null);

			unit.Id = _units.Add(unit);
			_network.SendUnitData(unit);
		}

		public void Remove(Unit unit)
		{
			Debug.Assert(unit != null);
			
			// Clean up the linked stuff
			unit.Structure = null;
			unit.World = null;

			// Actually remove it
			_units.Remove(unit.Id);
		}

		public void SendUnitDataFor(Player player)
		{
			Debug.Assert(player != null);

			// Send unit data for all units to the player
			foreach (var unit in player.Unit.World.Units)
				_network.SendUnitData(unit, player);
		}

		public void SendTeleportFor(Unit unit)
		{
			Debug.Assert(unit != null);
			_network.SendTeleport(unit);
		}

		public void Update(float delta)
		{
			// Send out physics update packets
			foreach (var unitEntry in _units)
				_network.SendUnitPhysics(unitEntry.Value);
		}
	}
}