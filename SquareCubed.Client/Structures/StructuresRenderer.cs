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

				// Iterate through chunks
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

							// Else, let's draw it (test grey tile for now)
							GL.Begin(PrimitiveType.Quads);

							GL.Color3(Color.Gray);
							GL.Vertex2(0 + x, 1 + y); // Left Top
							GL.Vertex2(0 + x, 0 + y); // Left Bottom
							GL.Vertex2(1 + x, 0 + y); // Right Bottom
							GL.Vertex2(1 + x, 1 + y); // Right Top

							GL.Color3(Color.FromArgb(64, 64, 64));
							if (tile.WallTypes[(int) WallSides.Top] != 0)
							{
								GL.Vertex2(0 + x, 1 + y); // Left Top
								GL.Vertex2(0 + x, 0.9f + y); // Left Bottom
								GL.Vertex2(1 + x, 0.9f + y); // Right Bottom
								GL.Vertex2(1 + x, 1 + y); // Right Top
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
