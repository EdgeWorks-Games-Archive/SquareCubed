using System;
using System.Diagnostics.Contracts;
using SquareCubed.Common.Utils;
using SquareCubed.Server.Players;
using SquareCubed.Server.Structures.Objects;

namespace SquareCubed.Server.Structures
{
	public class Structures
	{
		private readonly StructuresNetwork _network;
		private readonly AutoDictionary<ServerStructure> _structures = new AutoDictionary<ServerStructure>();

		public TypeRegistry<IServerObjectType> ObjectTypes { get; private set; }
		public ObjectNetwork ObjectNetwork { get; private set; }

		public Structures(Network.Network network)
		{
			ObjectTypes = new TypeRegistry<IServerObjectType>();
			ObjectNetwork = new ObjectNetwork(network);

			_network = new StructuresNetwork(network);
		}

		public void Add(ServerStructure structure)
		{
			Contract.Requires<ArgumentNullException>(structure != null);

			structure.Id = _structures.Add(structure);
			_network.SendStructureData(structure);
		}

		public void SendStructureDataFor(Player player)
		{
			Contract.Requires<ArgumentNullException>(player != null);

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