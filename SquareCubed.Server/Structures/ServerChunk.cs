using System;
using System.Diagnostics;
using Lidgren.Network;
using SquareCubed.Common.Data;

namespace SquareCubed.Server.Structures
{
	public class ServerChunk : Chunk
	{
	}

	public static class ServerChunkExtensions
	{
		public static void Write(this NetOutgoingMessage msg, ServerChunk chunk)
		{
			Debug.Assert(msg != null);
			Debug.Assert(chunk != null);

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