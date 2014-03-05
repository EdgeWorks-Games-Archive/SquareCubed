using System;
using System.Diagnostics.Contracts;
using Lidgren.Network;

namespace SquareCubed.Server.Players
{
	public class Player
	{
		public NetConnection Connection { get; private set; }
		public string Name { get; set; }
		public PlayerUnit Unit { get; set; }

		public Player(NetConnection connection, string name, PlayerUnit unit)
		{
			Contract.Requires<ArgumentNullException>(unit != null);

			Connection = connection;
			Name = name;

			// Set and Configure Unit Data
			Unit = unit;
			Unit.Player = this;
			Unit.World.UpdatePlayerEntry(this);
		}
	}
}