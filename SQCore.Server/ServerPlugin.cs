using OpenTK;
using SQCore.Common;
using SQCore.Server.Objects;
using SquareCubed.Common.Utils;
using SquareCubed.Server;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server
{
	public class ServerPlugin : CommonPlugin, IServerPlugin
	{
		#region Object Types

		private readonly PilotSeatObjectType _pilotSeatType;
		private readonly TeleporterObjectType _teleporterType;

		#endregion

		#region External Components

		private readonly TypeRegistry<IServerObjectType> _objectTypes;

		#endregion

		private readonly Chat.Chat _chat;

		public ServerPlugin(SquareCubed.Server.Server server)
		{
			Logger.LogInfo("Initializing Blink...");

			_objectTypes = server.Structures.ObjectTypes;

			// Set up the chat
			_chat = new Chat.Chat(server.Network, server.Players);

			// Add object types
			_pilotSeatType = new PilotSeatObjectType(server);
			_objectTypes.RegisterType(_pilotSeatType, 0);
			_teleporterType = new TeleporterObjectType(server.Structures.ObjectsNetwork, server.Players);
			_objectTypes.RegisterType(_teleporterType, 1);

			// Add the default spawn provider
			server.Players.AddSpawnProvider(new SpawnProvider(server, Logger));

			// Generate a new station
			var generator = new StationGenerator(_objectTypes);
			server.Structures.Add(generator.GenerateNew(server.Worlds.TestWorld, new Vector2(3, 4)));

			Logger.LogInfo("Finished initializing Blink!");
		}

		public void Dispose()
		{
			// Remove object types
			_objectTypes.UnregisterType(_pilotSeatType);
			_objectTypes.UnregisterType(_teleporterType);
		}
	}
}