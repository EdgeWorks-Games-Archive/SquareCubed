using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using OpenTK;

namespace SquareCubed.Common.Data
{
	public class Chunk
	{
		public const int ChunkSize = 32;

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

		public void SetTile(int x, int y, int type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x][y] == null)
				Tiles[x][y] = new Tile {Type = type};
			else
				Tiles[x][y].Type = type;
		}

		public void SetTopWall(int x, int y, int type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x][y] == null)
				Tiles[x][y] = new Tile {Type = 0};

			Tiles[x][y].WallTypes[(int) WallSides.Top] = type;
		}

		public void SetRightWall(int x, int y, int type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x][y] == null)
				Tiles[x][y] = new Tile {Type = 0};

			Tiles[x][y].WallTypes[(int) WallSides.Right] = type;
		}

		public void SetBottomWall(int x, int y, int type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y-1 >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x][y - 1] == null)
				Tiles[x][y - 1] = new Tile {Type = 0};

			Tiles[x][y - 1].WallTypes[(int) WallSides.Top] = type;
		}

		public void SetLeftWall(int x, int y, int type)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x-1 >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

			if (Tiles[x - 1][y] == null)
				Tiles[x - 1][y] = new Tile {Type = 0};

			Tiles[x - 1][y].WallTypes[(int) WallSides.Right] = type;
		}

		public void SetWalls(int x, int y, int top, int right, int bottom, int left)
		{
			Contract.Requires<ArgumentOutOfRangeException>(x - 1 >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(x < Tiles.Length);
			Contract.Requires<ArgumentOutOfRangeException>(y - 1 >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(y < Tiles[x].Length);

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
						Tiles[x][y].UpdateColliders(new Vector2(Position.X * ChunkSize + x, Position.Y * ChunkSize + y));
				}
			}
		}

		public IEnumerable<AaBb> GetColliders()
		{
			return (from tileRow in Tiles from tile in tileRow where tile != null select tile).SelectMany(t => t.WallColliders);
		}

		#endregion
	}
}