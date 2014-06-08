using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Structures
{
	internal class StructuresRenderer
	{
		private readonly Client _client;

		public StructuresRenderer(Client client)
		{
			_client = client;
		}

		private void RenderChunkTiles(ClientChunk chunk)
		{
			// Iterate through all TileTypes
			for (var x = 0; x < Chunk.ChunkSize; x++)
			{
				for (var y = 0; y < Chunk.ChunkSize; y++)
				{
					// If the tile is null or its type is 0 (walls only) ignore it
					var tile = chunk.Tiles[x][y];
					if (tile == null || tile.Type == 0) continue;

					// Get tile tile type and render it
					_client.Structures.TileTypes.GetType(tile.Type).RenderTile(new Vector2(x, y));
				}
			}
		}

		private void RenderObjects(ClientStructure structure)
		{
			foreach (var obj in structure.Objects)
			{
				obj.Render();
			}
		}

		private void RenderChunkWalls(ClientChunk chunk)
		{
			// Iterate through all TileTypes to render the Walls
			for (var x = 0; x < Chunk.ChunkSize; x++)
			{
				for (var y = 0; y < Chunk.ChunkSize; y++)
				{
					// If the tile is null, ignore it
					var tile = chunk.Tiles[x][y];
					if (tile == null) continue;

					// Else, let's draw its walls
					GL.Begin(PrimitiveType.Quads);

					// Walls are rendered all exactly the same except rotated.
					// Walls overlap in the corners but since they're solid grey that doesn't matter
					GL.Color3(Color.FromArgb(64, 64, 64));

					const float halfOffset = 3f/32f; // width / total

					// If the wall's type is set to 0 (means no wall) or 1 (means invisible), ignore it
					if (tile.WallTypes[(int) WallSides.Top] >= 2)
					{
						GL.Vertex2(x - halfOffset, y + 1f + halfOffset); // Left Top
						GL.Vertex2(x - halfOffset, y + 1f - halfOffset); // Left Bottom
						GL.Vertex2(x + 1f + halfOffset, y + 1f - halfOffset); // Right Bottom
						GL.Vertex2(x + 1f + halfOffset, y + 1f + halfOffset); // Right Top
					}
					if (tile.WallTypes[(int) WallSides.Right] >= 2)
					{
						GL.Vertex2(x + 1f + halfOffset, y + 1f + halfOffset); // Left Top
						GL.Vertex2(x + 1f - halfOffset, y + 1f + halfOffset); // Left Bottom
						GL.Vertex2(x + 1f - halfOffset, y + halfOffset); // Right Bottom
						GL.Vertex2(x + 1f + halfOffset, y + halfOffset); // Right Top
					}

					GL.End();
				}
			}
		}

		public void RenderStructures(IEnumerable<ClientStructure> structures)
		{
			GL.MatrixMode(MatrixMode.Modelview);
			foreach (var structure in structures)
			{
				GL.PushMatrix();

				// Move to the structure's 0,0 and rotate around it
				GL.Translate(structure.Position.X, structure.Position.Y, 0);
				GL.Rotate(MathHelper.RadiansToDegrees(structure.Rotation), 0, 0, 1);

				// Iterate through chunks to render the Ground and Objects
				foreach (var chunk in structure.Chunks)
				{
					// Translate to the chunk position
					GL.PushMatrix();
					GL.Translate(chunk.Position.X*Chunk.ChunkSize, chunk.Position.Y*Chunk.ChunkSize, 0);

					RenderChunkTiles(chunk);

					GL.PopMatrix();
				}

				RenderObjects(structure);

				// Units are rendered below walls but above tiles and objects
				_client.Units.RenderFor(structure);

				// Iterate through chunks to render the Walls.
				// Walls have to be done after all the ground of all chunks is done,
				// which is why this is a separate loop from the ground loop.
				foreach (var chunk in structure.Chunks)
				{
					// Translate to the chunk position
					GL.PushMatrix();
					GL.Translate(chunk.Position.X*Chunk.ChunkSize, chunk.Position.Y*Chunk.ChunkSize, 0);

					RenderChunkWalls(chunk);

					GL.PopMatrix();
				}

				GL.PopMatrix();
			}
		}
	}
}