using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Data;

namespace SquareCubed.Client.Structures
{
	class StructuresRenderer
	{
		public void RenderStructures(IEnumerable<Structure> structures)
		{
			GL.MatrixMode(MatrixMode.Modelview);
			foreach (var structure in structures)
			{
				// Translate to the ship position
				GL.PushMatrix();
				GL.Translate(structure.Position.X, structure.Position.Y, 0);

				// Iterate through chunks to render the Ground
				foreach (var chunk in structure.Chunks)
				{
					// Translate to the chunk position
					GL.PushMatrix();
					GL.Translate(chunk.X * Chunk.ChunkSize, chunk.Y * Chunk.ChunkSize, 0);

					// Iterate through all Tiles
					for (var x = 0; x < Chunk.ChunkSize; x++)
					{
						for (var y = 0; y < Chunk.ChunkSize; y++)
						{
							// If the tile is null, ignore it
							var tile = chunk.Tiles[x][y];
							if (tile == null) continue;

							// If the tile's ground is set to 0 (means no ground) or 1 (means invisible), ignore it as well
							if (tile.Type < 2) continue;

							// Else, let's draw it (test grey tile for now)
							GL.Begin(PrimitiveType.Quads);

							GL.Color3(Color.Gray);
							GL.Vertex2(x + 0, y + 1); // Left Top
							GL.Vertex2(x + 0, y + 0); // Left Bottom
							GL.Vertex2(x + 1, y + 0); // Right Bottom
							GL.Vertex2(x + 1, y + 1); // Right Top

							GL.End();
						}
					}

					GL.PopMatrix();
				}

				// Iterate through chunks to render the Walls
				// Walls have to be done after all the ground of all
				// chunks is done, which is why this is a separate
				// loop from the ground loop.
				foreach (var chunk in structure.Chunks)
				{
					// Translate to the chunk position
					GL.PushMatrix();
					GL.Translate(chunk.X * Chunk.ChunkSize, chunk.Y * Chunk.ChunkSize, 0);

					// Iterate through all Tiles to render the Walls
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

							// If the wall's type is set to 0 (means no wall) or 1 (means invisible), ignore it
							if (tile.WallTypes[(int)WallSides.Top] >= 2)
							{
								GL.Vertex2(x - 0.1f, y + 1.1f); // Left Top
								GL.Vertex2(x - 0.1f, y + 0.9f); // Left Bottom
								GL.Vertex2(x + 1.1f, y + 0.9f); // Right Bottom
								GL.Vertex2(x + 1.1f, y + 1.1f); // Right Top
							}
							if (tile.WallTypes[(int)WallSides.Right] >= 2)
							{
								GL.Vertex2(x + 1.1f, y + 1.1f); // Left Top
								GL.Vertex2(x + 0.9f, y + 1.1f); // Left Bottom
								GL.Vertex2(x + 0.9f, y + 0.1f); // Right Bottom
								GL.Vertex2(x + 1.1f, y + 0.1f); // Right Top
							}

							GL.End();
						}
					}

					GL.PopMatrix();
				}

				GL.PopMatrix();
			}
		}
	}
}
