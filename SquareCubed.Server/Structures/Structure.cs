using System.Collections.Generic;
using OpenTK;
using SquareCubed.Data;

namespace SquareCubed.Server.Structures
{
	internal class Structure
	{
		public Structure()
		{
			Chunks = new List<Chunk>();
		}

		public uint Id { get; set; }
		public List<Chunk> Chunks { get; set; }
		public Vector2 Position { get; set; }
	}
}