using Lidgren.Network;

namespace SquareCubed.Data
{
	public class Tile
	{
		public enum WallSides
		{
			Top,
			Right,
			Bottom,
			Left
		}

		public Tile()
		{
			WallTypes = new uint[4];
		}

		public uint Type { get; set; } // Reserved types: 0 = None, 1 = Invisible (Used for docking)
		public uint[] WallTypes { get; set; } // Reserved types: 0 = None
	}

	public static class TileExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Tile tile)
		{
			msg.Write(tile.Type);
			foreach (var type in tile.WallTypes)
				msg.Write(type);
		}

		public static Tile ReadTile(this NetIncomingMessage msg)
		{
			var tile = new Tile { Type = msg.ReadUInt32() };
			for (var i = 0; i < tile.WallTypes.Length; i++)
				tile.WallTypes[i] = msg.ReadUInt32();

			return tile;
		}
	}
}