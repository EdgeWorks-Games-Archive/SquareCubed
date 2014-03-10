using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lidgren.Network;
using OpenTK;

namespace SquareCubed.Common.Data
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
		public Vector2i Position { get; set; }

		#region Tile Editing Functions

		public void SetTile(uint x, uint y, uint type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x][y] == null)
				Tiles[x][y] = new Tile {Type = type};
			else
				Tiles[x][y].Type = type;
		}

		public void SetTopWall(uint x, uint y, uint type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x][y] == null)
				Tiles[x][y] = new Tile {Type = 0};

			Tiles[x][y].WallTypes[(int) WallSides.Top] = type;
		}

		public void SetRightWall(uint x, uint y, uint type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x][y] == null)
				Tiles[x][y] = new Tile {Type = 0};

			Tiles[x][y].WallTypes[(int) WallSides.Right] = type;
		}

		public void SetBottomWall(uint x, uint y, uint type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x][y - 1] == null)
				Tiles[x][y - 1] = new Tile {Type = 0};

			Tiles[x][y - 1].WallTypes[(int) WallSides.Top] = type;
		}

		public void SetLeftWall(uint x, uint y, uint type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x - 1][y] == null)
				Tiles[x - 1][y] = new Tile {Type = 0};

			Tiles[x - 1][y].WallTypes[(int) WallSides.Right] = type;
		}

		public void SetWalls(uint x, uint y, uint top, uint right, uint bottom, uint left)
		{
			SetTopWall(x, y, top);
			SetRightWall(x, y, right);
			SetBottomWall(x, y, bottom);
			SetLeftWall(x, y, left);
		}

		#endregion

		#region Collider Helper Functions

		public void UpdateColliders()
		{
			for (var x = 0; x < ChunkSize; x++)
			{
				for (var y = 0; y < ChunkSize; y++)
				{
					if(Tiles[x][y] != null)
						Tiles[x][y].UpdateColliders(new Vector2(Position.X * (int)ChunkSize + x, Position.Y * (int)ChunkSize + y));
				}
			}
		}

		public IEnumerable<AaBb> GetColliders()
		{
			return (from tileRow in Tiles from tile in tileRow where tile != null select tile).SelectMany(t => t.WallColliders);
		}

		#endregion
	}

	public static class ChunkExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Chunk chunk)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Requires<ArgumentNullException>(chunk != null);

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
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Ensures(Contract.Result<Chunk>() != null);

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
			chunk.UpdateColliders();
			return chunk;
		}
	}
}