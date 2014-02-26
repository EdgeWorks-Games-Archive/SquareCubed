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
							if (chunk.Tiles[x][y] == null) continue;

							// Else, let's draw it (test grey tile for now)
							GL.Begin(PrimitiveType.Quads);

							GL.Color3(Color.Gray);
							GL.Vertex2(0 + x, 1 + y); // Left Top
							GL.Vertex2(0 + x, 0 + y); // Left Bottom
							GL.Vertex2(1 + x, 0 + y); // Right Bottom
							GL.Vertex2(1 + x, 1 + y); // Right Top

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
