using Lidgren.Network;
using SquareCubed.Server.Players;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	class TeleporterObject : NetworkServerObjectBase
	{
		private readonly Players _players;
		private readonly TeleporterObjectType _type;

		public ServerStructure Parent { get; private set; }

		public TeleporterObject(TeleporterObjectType type, ObjectsNetwork network, Players players, ServerStructure parent)
			: base(type, network)
		{
			_type = type;
			_players = players;
			Parent = parent;

			// TODO: Remove on dispose
			_type.AddTeleporter(this);
		}

		public override void OnMessage(NetIncomingMessage msg)
		{
			var obj = _type.GetRandomTeleporter(this);
			_players[msg.SenderConnection].Unit.Teleport(obj.Parent, obj.Position);
		}
	}
}
