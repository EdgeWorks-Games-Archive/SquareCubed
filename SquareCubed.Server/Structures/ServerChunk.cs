using System.Diagnostics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Lidgren.Network;
using SquareCubed.Common.Data;

namespace SquareCubed.Server.Structures
{
	public class ServerChunk : Chunk
	{
		public void AddShapesTo(Body body)
		{
			// TODO: Generate more efficient by linking together tiles if possible.
			// Linking tiles into rows perhaps would improve it.
			var chunkOffset = new Vector2i(ChunkSize * Position.X, ChunkSize * Position.Y);
			
			for (var x = 0; x < ChunkSize; x++)
			{
				for (var y = 0; y < ChunkSize; y++)
				{
					if (Tiles[x][y] == null || Tiles[x][y].Type == 0)
						continue;

					var vertices = new Vertices
					{
						new Microsoft.Xna.Framework.Vector2(chunkOffset.X + x, chunkOffset.Y + y + 1), // Left Top
						new Microsoft.Xna.Framework.Vector2(chunkOffset.X + x, chunkOffset.Y + y), // Left Bottom
						new Microsoft.Xna.Framework.Vector2(chunkOffset.X + x + 1, chunkOffset.Y + y), // Right Bottom
						new Microsoft.Xna.Framework.Vector2(chunkOffset.X + x + 1, chunkOffset.Y + y + 1), // Right Top
					};
					var shape = new PolygonShape(vertices, 1.0f);
					body.CreateFixture(shape);
				}
			}
		}
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