using System;
using System.Diagnostics.Contracts;
using Lidgren.Network;

namespace SquareCubed.Common.Data
{
	public enum WallSides
	{
		Top,
		Right
	}

	public class Tile
	{
		public Tile()
		{
			WallTypes = new uint[2];
		}

		public uint Type { get; set; } // Reserved types: 0 = None, 1 = Invisible (Used for docking)
		public uint[] WallTypes { get; set; } // Reserved types: 0 = None, 1 = Invisible (Used for doors)
	}

	public static class TileExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Tile tile)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Requires<ArgumentNullException>(tile != null);

			msg.Write(tile.Type);
			foreach (var type in tile.WallTypes)
				msg.Write(type);
		}

		public static Tile ReadTile(this NetIncomingMessage msg)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Ensures(Contract.Result<Tile>() != null);

			var tile = new Tile {Type = msg.ReadUInt32()};
			for (var i = 0; i < tile.WallTypes.Length; i++)
				tile.WallTypes[i] = msg.ReadUInt32();

			return tile;
		}
	}
}