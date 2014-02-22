using Lidgren.Network;
using SquareCubed.Network;

namespace SquareCubed.Server.Units
{
	internal class UnitsNetwork
	{
		private readonly Server _server;

		public UnitsNetwork(Server server)
		{
			_server = server;
		}

		public void SendUnitData(Unit unit)
		{
			var msg = _server.Network.Peer.CreateMessage();

			// Add the packet type Id
			// TODO: loop up what the Id actually is instead of having it set here
			msg.Write((ushort)3);
			msg.WritePadBits();

			// The client knows what unit to update using the Id
			msg.Write(unit.Id);

			// Send the data to all players in the world the unit is in
			unit.World.SendToAllPlayers(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.UnitData);
		}
	}
}