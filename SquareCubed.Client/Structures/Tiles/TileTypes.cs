using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Structures.Tiles
{
	public class TileTypes
	{
		public const uint MaxId = 20;
		private readonly TileType[] _typeList;

		public TileTypes()
		{
			_typeList = new TileType[MaxId + 1];
			_typeList[1] = new InvisibleTileType(this);
		}

		public TileType GetType(uint id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Type Id is bigger than the maximum Id allowed.");

			return _typeList[id];
		}

		public void RegisterType(TileType type, uint id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Type Id is bigger than the maximum Id allowed.");

			if (_typeList[id] != null)
				throw new Exception("Tile type already registered!");

			_typeList[id] = type;
		}
	}
}