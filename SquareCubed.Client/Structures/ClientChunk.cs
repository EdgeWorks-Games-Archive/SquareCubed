using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Common.Data;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Structures
{
	public class ClientChunk : Chunk
	{
		public List<IClientObject> Objects { get; set; }
	}

	public static class ClientChunkExtensions
	{
		private static List<IClientObject> ReadObjects(this NetIncomingMessage msg, TypeRegistry<IClientObjectType> objectTypes)
		{
			var amount = msg.ReadInt32();
			var objects = new List<IClientObject>(amount);
			for (var i = 0; i < amount; i++)
			{
				// Create an object of the type with the id we received assigned to it.
				var obj = objectTypes.GetType(msg.ReadInt32()).CreateNew();
				obj.Position = msg.ReadVector2();
				objects.Add(obj);
			}

			return objects;
		}

		public static ClientChunk ReadChunk(this NetIncomingMessage msg, TypeRegistry<IClientObjectType> objectTypes)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Ensures(Contract.Result<ClientChunk>() != null);

			var chunk = new ClientChunk();

			// Read all the tiles from the message
			for (var x = 0; x < Chunk.ChunkSize; x++)
			{
				for (var y = 0; y < Chunk.ChunkSize; y++)
				{
					// False means no tile, so ignore it
					if (!msg.ReadBoolean()) continue;

					msg.ReadPadBits();
					chunk.Tiles[x][y] = msg.ReadTile();
				}
			}
			msg.ReadPadBits();
			chunk.UpdateColliders();

			// Read all the objects from the message
			chunk.Objects = msg.ReadObjects(objectTypes);

			return chunk;
		}
	}
}