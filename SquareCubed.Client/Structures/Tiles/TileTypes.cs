using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Structures.Tiles
{
	public class TileTypes
	{
		public TileType[] TypeList { get; private set; }

		public TileTypes()
		{
			TypeList = new TileType[20];
			TypeList[1] = new InvisibleTileType(this);
		}

		public void RegisterType(TileType type, uint id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				id < TypeList.Length,
				"Type Id is bigger than the amount of allocated type Id slots.");

			if (TypeList[id] != null)
				throw new Exception("Tile type already registered!");

			TypeList[id] = type;
		}
	}
}
