using System.Collections.Generic;

namespace SquareCubed.Client.Structures
{
	public class Structures
	{
		private readonly StructuresNetwork _network;
		private readonly StructuresRenderer _renderer;
		private readonly Dictionary<uint, Structure> _structures = new Dictionary<uint, Structure>();

		public Tiles.TileTypes TileTypes { get; private set; }

		public Structures(Client client)
		{
			TileTypes = new Tiles.TileTypes();
			_network = new StructuresNetwork(client, this);
			_renderer = new StructuresRenderer(client);
		}

		public void OnStructureData(Structure structure)
		{
			Structure oldStructure;

			// Try to get the unit, if we can't we need to add it, otherwise overwrite it
			if (!_structures.TryGetValue(structure.Id, out oldStructure))
				_structures.Add(structure.Id, structure);
			else
				_structures[structure.Id] = structure;
		}

		public void Render()
		{
			_renderer.RenderStructures(_structures.Values);
		}
	}
}