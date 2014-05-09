using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Common.Data;
using SquareCubed.Server.Structures.Objects;

namespace SquareCubed.Server.Structures
{
	public class ServerChunk : Chunk
	{
		private readonly List<ServerObject> _objects = new List<ServerObject>();

		public IReadOnlyCollection<ServerObject> Objects { get { return _objects.AsReadOnly(); } }

		// TODO: Change to use proper object classes instead of Ids and resolve Ids on send instead
		public void AddObject(float x, float y, int id)
		{
			_objects.Add(new ServerObject
			{
				Position = new Vector2(x, y),
				Id = id
			});
		}
	}

	public static class ServerChunkExtensions
	{
		public static void Write(this NetOutgoingMessage msg, ServerChunk chunk)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Requires<ArgumentNullException>(chunk != null);

			// Write all the tiles to the message
			for (var x = 0; x < Chunk.ChunkSize; x++)
			{
				for (var y = 0; y < Chunk.ChunkSize; y++)
				{
					if (chunk.Tiles[x][y] != null)
					{
						msg.Write(true);
						msg.WritePadBits();
						msg.Write(chunk.Tiles[x][y]);
					}
					else
						msg.Write(false);
				}
			}
			msg.WritePadBits();

			// Write all the objects to the message
			msg.Write(chunk.Objects.Count);
			foreach (var obj in chunk.Objects)
			{
				msg.Write(obj.Id);
				msg.Write(obj.Position);
			}
		}
	}
}
