using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Structures.Tiles
{
	public class TileTypes
	{
		private const uint MaxId = 20;
		private readonly TileType[] _typeList;

		public TileTypes()
		{
			_typeList = new TileType[MaxId + 1];
			_typeList[1] = new InvisibleTileType(this);
		}

		public TileType GetType(int id)
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

			// Overwriting the type could lead to serious issues.
			// Preventing the type from being overwritten even at
			// runtime will give us a clearer bugreport.
			if (_typeList[id] != null)
				throw new Exception("Tile type already registered!");

			_typeList[id] = type;
		}

		public void UnregisterType(TileType type)
		{
			var index = Array.IndexOf(_typeList, type);
			_typeList[index] = null;
		}
	}
}