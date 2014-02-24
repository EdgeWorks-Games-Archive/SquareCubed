using OpenTK;
using SquareCubed.Data;
using SquareCubed.Utils;

namespace SquareCubed.Server.Structures
{
	class Structures
	{
		private readonly AutoDictionary<Structure> _structures = new AutoDictionary<Structure>();

		public Structures()
		{
			// Create a test ship
			var str = new Structure
			{
				Position = new Vector2(-5.5f, -5.5f)
			};

			// Add a single chunk with a single tile
			var chunk = new Chunk();
			chunk.Tiles[5][5].Type = 1;
			str.Chunks.Add(chunk);

			// And add the ship to the collection
			Add(str);
		}

		public void Add(Structure structure)
		{
			structure.Id = _structures.Add(structure);
			// TODO: _network.SendStructureData(structure);
		}
	}
}