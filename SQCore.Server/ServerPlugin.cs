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
			_teleporterType = new TeleporterObjectType();
			_objectTypes.RegisterType(_teleporterType, 1);

			// Add the default spawn provider
			server.Players.AddSpawnProvider(new SpawnProvider(server, Logger));

			// Add a little test structure
			var str = new ServerStructure
			{
				World = server.Worlds.TestWorld,
				Position = new Vector2(3, 4),
				Rotation = 0.0f,
				Center = new Vector2(5.5f, 5.5f)
			};

			// Add a chunk for the test structure
			var chunk = new ServerChunk();
			chunk.SetTile(5, 5, 3);
			chunk.SetLeftWall(5, 5, 2);
			chunk.SetBottomWall(5, 5, 2);

			chunk.SetTile(6, 5, 3);
			chunk.SetRightWall(6, 5, 2);
			chunk.SetBottomWall(6, 5, 2);

			chunk.SetTile(6, 6, 3);
			chunk.SetRightWall(6, 6, 2);
			chunk.SetTopWall(6, 6, 2);

			chunk.SetTile(5, 6, 3);
			chunk.SetLeftWall(5, 6, 2);

			chunk.SetTile(5, 7, 3);
			chunk.SetWalls(5, 7, 2, 2, 0, 2);
			str.AddObject(5.5f, 7.5f, 1, _objectTypes);

			str.Chunks.Add(chunk);

			// Add the structure to the world
			server.Structures.Add(str);

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