using System;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using SquareCubed.Common.Utils;
using SquareCubed.Server.Worlds;

namespace SquareCubed.Server.Players
{
	public class Player
	{
		public Player(NetConnection connection, string name, PlayerUnit unit)
		{
			Contract.Requires<ArgumentNullException>(unit != null);

			Connection = connection;
			Name = name;

			WorldLink = new ParentLink<World, Player>(this, w => w.Players);

			// Set and Configure Unit Data
			Unit = unit;
			Unit.Player = this;
		}

		public NetConnection Connection { get; private set; }
		public string Name { get; set; }
		public PlayerUnit Unit { get; set; }
		public ParentLink<World, Player> WorldLink { get; private set; }

		public World World
		{
			get { return WorldLink.Property; }
			set
			{
				WorldLink.Property = value;
				Unit.WorldLink.Property = value;
			}
		}
	}
}