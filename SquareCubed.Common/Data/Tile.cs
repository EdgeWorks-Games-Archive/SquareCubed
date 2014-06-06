using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private List<AaBb> _wallColliders;

		public Tile()
		{
			WallTypes = new int[2];
		}

		public int Type { get; set; } // Reserved types: 0 = None, 1 = Invisible (Used for docking)
		public int[] WallTypes { get; set; } // Reserved types: 0 = None, 1 = Invisible (Used for doors)

		public IEnumerable<AaBb> WallColliders
		{
			get { return _wallColliders.AsReadOnly(); }
		}

		/// <summary>
		///     Updates the wall colliders for this tile.
		/// </summary>
		/// <param name="tilePosition">The tile's position (bottom left of the tile) relative to the structure.</param>
		public void UpdateColliders(Vector2 tilePosition)
		{
			const float halfOffset = 1f/64f*6f;

			_wallColliders = new List<AaBb>();

			if (WallTypes[(int) WallSides.Top] != 0)
			{
				_wallColliders.Add(new AaBb
				{
					Position = tilePosition + new Vector2(-halfOffset, 1f - halfOffset),
					Size = new Vector2(1f + (halfOffset*2f), halfOffset*2f)
				});
			}
			if (WallTypes[(int) WallSides.Right] != 0)
			{
				_wallColliders.Add(new AaBb
				{
					Position = tilePosition + new Vector2(1f - halfOffset, -halfOffset),
					Size = new Vector2(halfOffset*2f, 1f + (halfOffset*2f))
				});
			}
		}
	}

	public static class TileExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Tile tile)
		{
			Debug.Assert(msg != null);
			Debug.Assert(tile != null);

			msg.Write(tile.Type);
			foreach (var type in tile.WallTypes)
				msg.Write(type);
		}

		public static Tile ReadTile(this NetIncomingMessage msg)
		{
			Debug.Assert(msg != null);

			var tile = new Tile {Type = msg.ReadInt32()};
			for (var i = 0; i < tile.WallTypes.Length; i++)
				tile.WallTypes[i] = msg.ReadInt32();

			Debug.Assert(tile != null);
			return tile;
		}
	}
}