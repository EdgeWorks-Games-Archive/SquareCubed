using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Structures
{
	public class ClientChunk : Chunk
	{
		public List<ClientObject> Objects { get; set; } 
	}

	public static class ClientChunkExtensions
	{
		private static List<ClientObject> ReadObjects(this NetIncomingMessage msg, ObjectTypes objectTypes)
		{
			var amount = msg.ReadInt32();
			var objects = new List<ClientObject>(amount);
			for (var i = 0; i < amount; i++)
			{
				var obj = (ClientObject)Activator.CreateInstance(objectTypes.TypeList[msg.ReadUInt32()]);
				obj.Position = msg.ReadVector2();
				objects.Add(obj);
			}

			return objects;
		}

		public static ClientChunk ReadChunk(this NetIncomingMessage msg, ObjectTypes objectTypes)
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
