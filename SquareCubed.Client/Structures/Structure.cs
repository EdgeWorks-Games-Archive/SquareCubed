using System.Collections.Generic;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Data;

namespace SquareCubed.Client.Structures
{
	public class Structure
	{
		public uint Id { get; set; }
		public List<Chunk> Chunks { get; set; }
		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public Vector2 Center { get; set; }
	}

	public static class StructureExtensions
	{
		private static List<Chunk> ReadChunks(this NetIncomingMessage msg)
		{
			var amount = msg.ReadInt32();
			var chunks = new List<Chunk>(amount);
			for (var i = 0; i < amount; i++)
				chunks.Add(msg.ReadChunk());

			return chunks;
		}

		public static Structure ReadStructure(this NetIncomingMessage msg)
		{
			return new Structure
			{
				Id = msg.ReadUInt32(),
				Position = msg.ReadVector2(),
				Rotation = msg.ReadFloat(),
				Center = msg.ReadVector2(),
				Chunks = msg.ReadChunks()
			};
		}
	}
}
