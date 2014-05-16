using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Common.Data;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Structures
{
	public class ClientChunk : Chunk
	{
	}

	public static class ClientChunkExtensions
	{
		public static ClientChunk ReadChunk(this NetIncomingMessage msg)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Ensures(Contract.Result<ClientChunk>() != null);

			var chunk = new ClientChunk();

			// Read all the tiles from the message
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
			msg.ReadPadBits();
			chunk.UpdateColliders();

			return chunk;
		}
	}
}