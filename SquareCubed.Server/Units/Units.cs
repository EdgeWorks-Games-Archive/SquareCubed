using SquareCubed.Server.Players;
using SquareCubed.Utils;

namespace SquareCubed.Server.Units
{
	public class Units
	{
		private readonly Server _server;
		private readonly AutoDictionary<Unit> _units = new AutoDictionary<Unit>();
		private readonly UnitsNetwork _network;

		public Units(Server server)
		{
			_server = server;
			_network = new UnitsNetwork(_server);
		}

		public void Add(Unit unit)
		{
			unit.Id = _units.Add(unit);
			_network.SendUnitData(unit);
		}

		public void SendUnitDataFor(Player player)
		{
			foreach (var unit in player.Unit.World.Units)
				_network.SendUnitData(unit);
		}

		public void Update(float delta)
		{
			// Send out physics update packets
			foreach (var unitEntry in _units)
			{
				_network.SendUnitPhysics(unitEntry.Value);
			}
		}
	}
}