using System;
using System.Diagnostics.Contracts;
using OpenTK;
using SquareCubed.Client.Units;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Player
{
	internal class PlayerUnit : Unit
	{
		internal PlayerUnit(Unit oldUnit)
			: base(oldUnit)
		{
			Contract.Requires<ArgumentNullException>(oldUnit != null);
		}

		public AaBb AaBb
		{
			get
			{
				return new AaBb
				{
					Position = Position - new Vector2(0.3f, 0.3f),
					Size = new Vector2(0.6f, 0.6f)
				};
			}
		}

		public override void ProcessPhysicsPacketData(Vector2 position)
		{
			// Player doesn't care about update packets
		}
	}
}