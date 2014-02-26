﻿using Lidgren.Network;

namespace SquareCubed.Data
{
	public class Chunk
	{
		public const uint ChunkSize = 16;

		public Chunk()
		{
			// Initialize tiles array
			Tiles = new Tile[ChunkSize][];
			for (var i = 0; i < Tiles.Length; i++)
				Tiles[i] = new Tile[ChunkSize];
		}

		public Tile[][] Tiles { get; private set; }

		#region Position

		// OpenTK doesn't have an int version of Vector2

		public int X { get; set; }
		public int Y { get; set; }

		#endregion
	}

	public static class ChunkExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Chunk chunk)
		{
			for (var x = 0; x < Chunk.ChunkSize; x++)
			{
				for (var y = 0; y < Chunk.ChunkSize; y++)
				{
					if (chunk.Tiles[x][y] != null)
					{
						msg.Write(true);
						msg.WritePadBits();
						msg.Write(chunk.Tiles[x][y]);
					}
					else
						msg.Write(false);
				}
			}
		}

		public static Chunk ReadChunk(this NetIncomingMessage msg)
		{
			var chunk = new Chunk();
			for (var x = 0; x < Chunk.ChunkSize; x++)
			{
				for (var y = 0; y < Chunk.ChunkSize; y++)
				{
					// False means no tile, so ignore it
					if (!msg.ReadBoolean()) continue;

					msg.ReadPadBits();
					chunk.Tiles[x][y] = msg.ReadTile();
				}
			}
			return chunk;
		}
	}
}