using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Common.Data;
using SquareCubed.Server.Structures.Objects;

namespace SquareCubed.Server.Structures
{
	public class ServerChunk : Chunk
	{
	}

	public static class ServerChunkExtensions
	{
		public static void Write(this NetOutgoingMessage msg, ServerChunk chunk)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Requires<ArgumentNullException>(chunk != null);

			// Write all the tiles to the message
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
			msg.WritePadBits();
		}
	}
}
