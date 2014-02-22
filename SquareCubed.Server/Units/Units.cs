using Lidgren.Network;
using SquareCubed.Network;
using SquareCubed.Utils;

namespace SquareCubed.Server.Units
{
	public class Units
	{
		private readonly Server _server;
		private readonly AutoDictionary<Unit> _units = new AutoDictionary<Unit>();
		private readonly UnitsNetwork _network;

		public Units(Server server)
		{
			_server = server;
			_network = new UnitsNetwork(_server);
		}

		public void Add(Unit unit)
		{
			unit.Id = _units.Add(unit);
			_network.SendUnitData(unit);
		}

		public void Update(float delta)
		{
			// Send out physics update packets
			foreach (var unitEntry in _units)
			{
				// Write data for packet
				var msg = _server.Network.Peer.CreateMessage();
				msg.Write((ushort)2);
				msg.WritePadBits();
				msg.Write(unitEntry.Key);
				unitEntry.Value.WritePositionData(msg); // TODO: Change to write physics data instead of just position

				// Send data to appropriate players
				unitEntry.Value.World.SendToAllPlayers(
					msg,
					NetDeliveryMethod.UnreliableSequenced,
					(int) SequenceChannels.UnitPhysics);
			}
		}
	}
}