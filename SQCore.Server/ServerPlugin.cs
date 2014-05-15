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

		#endregion

		#region External Components

		private TypeRegistry<IServerObjectType> _objectTypes;

		#endregion

		private readonly Chat.Chat _chat;

		public ServerPlugin(SquareCubed.Server.Server server)
		{
			Logger.LogInfo("Initializing core plugin...");

			_objectTypes = server.Structures.ObjectTypes;

			// Set up the chat
			_chat = new Chat.Chat(server.Network, server.Players);

			// Add object types
			_pilotSeatType = new PilotSeatObjectType();
			_objectTypes.RegisterType(_pilotSeatType, 0);

			// Add the default spawn provider
			server.Players.AddSpawnProvider(new SpawnProvider(server, Logger));

			// Add a little test structure
			var str = new Structure
			{
				World = server.Worlds.TestWorld,
				Position = new Vector2(3, 2),
				Rotation = 0.0f,
				Center = new Vector2(5.5f, 5.5f)
			};

			// Add a chunk for the test structure
			var chunk = new ServerChunk();
			chunk.SetTile(5, 5, 3);
			chunk.SetWalls(5, 5, 2, 2, 2, 2);
			str.Chunks.Add(chunk);

			// Add the structure to the world
			server.Structures.Add(str);

			Logger.LogInfo("Finished initializing core plugin!");
		}

		public void Dispose()
		{
		}
	}
}