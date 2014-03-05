using SquareCubed.Server.Players;
using SquareCubed.Utils;

namespace SquareCubed.Server.Structures
{
	public class Structures
	{
		private readonly StructuresNetwork _network;
		private readonly AutoDictionary<Structure> _structures = new AutoDictionary<Structure>();

		public Structures(Server server)
		{
			_network = new StructuresNetwork(server);
		}

		public void Add(Structure structure)
		{
			structure.Id = _structures.Add(structure);
			_network.SendStructureData(structure);
		}

		public void SendStructureDataFor(Player player)
		{
			// Send structure data for all structures to the player
			foreach (var structure in player.Unit.World.Structures)
				_network.SendStructureData(structure, player);
		}

		public void Update(float delta)
		{
			// Send out physics update packets
			foreach (var structureEntry in _structures)
				_network.SendStructurePhysics(structureEntry.Value);
		}
	}
}