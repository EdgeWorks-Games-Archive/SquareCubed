using System;
using System.Diagnostics.Contracts;
using SquareCubed.Server.Players;
using SquareCubed.Utils;

namespace SquareCubed.Server.Units
{
	public class Units
	{
		private readonly AutoDictionary<Unit> _units = new AutoDictionary<Unit>();
		private readonly UnitsNetwork _network;

		public Units(Server server)
		{
			_network = new UnitsNetwork(server);
		}

		public void Add(Unit unit)
		{
			Contract.Requires<ArgumentNullException>(unit != null);

			unit.Id = _units.Add(unit);
			_network.SendUnitData(unit);
		}

		public void SendUnitDataFor(Player player)
		{
			Contract.Requires<ArgumentNullException>(player != null);

			// Send unit data for all units to the player
			foreach (var unit in player.Unit.World.Units)
				_network.SendUnitData(unit, player);
		}

		public void Update(float delta)
		{
			// Send out physics update packets
			foreach (var unitEntry in _units)
				_network.SendUnitPhysics(unitEntry.Value);
		}
	}
}