using System;
using OpenTK;
using SquareCubed.Common.Utils;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Structures.Objects;
using SquareCubed.Server.Worlds;

namespace SQCore.Server
{
	internal class StationGenerator
	{
		private readonly TypeRegistry<IServerObjectType> _objTypes;
		private readonly Random _random = new Random();

		public StationGenerator(TypeRegistry<IServerObjectType> objTypes)
		{
			_objTypes = objTypes;
		}

		public ServerStructure GenerateNew(World world, Vector2 position)
		{
			// Add a little test structure
			var str = new ServerStructure(new Vector2(6, 6))
			{
				World = world,
				Position = position
			};

			// Add a chunk for the test structure
			var chunk = new ServerChunk();
			str.Chunks.Add(chunk);

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
			str.AddObject(5.5f, 7.5f, 1, _objTypes);

			str.RegenerateShapes();

			return str;
		}
	}
}