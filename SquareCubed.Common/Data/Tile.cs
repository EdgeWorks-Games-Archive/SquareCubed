using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using OpenTK;

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

		/// <summary>
		/// Updates the wall colliders for this tile.
		/// </summary>
		/// <param name="tilePosition">The tile's position (bottom left of the tile) relative to the structure.</param>
		public void UpdateColliders(Vector2 tilePosition)
		{
			_wallColliders = new List<AaBb>();

			if (WallTypes[(int) WallSides.Top] != 0)
			{
				_wallColliders.Add(new AaBb
				{
					Position = tilePosition + new Vector2(-0.1f, 0.9f),
					Size = new Vector2(1.2f, 0.2f)
				});
			}
			if (WallTypes[(int) WallSides.Right] != 0)
			{
				_wallColliders.Add(new AaBb
				{
					Position = tilePosition + new Vector2(0.9f, -0.1f),
					Size = new Vector2(0.2f, 1.2f)
				});
			}
		}

		private List<AaBb> _wallColliders;

		public uint Type { get; set; } // Reserved types: 0 = None, 1 = Invisible (Used for docking)
		public uint[] WallTypes { get; set; } // Reserved types: 0 = None, 1 = Invisible (Used for doors)
		public IEnumerable<AaBb> WallColliders
		{
			get { return _wallColliders.AsReadOnly(); }
		}
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