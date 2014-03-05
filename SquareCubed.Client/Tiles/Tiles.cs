using System;

namespace SquareCubed.Client.Tiles
{
	public class Tiles
	{
		public TileType[] TileList { get; private set; }

		public Tiles()
		{
			TileList = new TileType[20];
			TileList[1] = new InvisibleTileType(this);
		}

		public void RegisterType(TileType type, uint id)
		{
			if(TileList[id] != null)
				throw new Exception("Tile type already registered!");

			TileList[id] = type;
		}
	}
}
