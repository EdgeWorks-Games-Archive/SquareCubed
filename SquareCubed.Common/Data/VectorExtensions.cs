﻿using System;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using OpenTK;

namespace SquareCubed.Common.Data
{
	public static class VectorExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Vector2 vector)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Requires<ArgumentNullException>(vector != null);

			msg.Write(vector.X);
			msg.Write(vector.Y);
		}

		public static Vector2 ReadVector2(this NetIncomingMessage msg)
		{
			Contract.Requires<ArgumentNullException>(msg != null);

			return new Vector2
			{
				X = msg.ReadFloat(),
				Y = msg.ReadFloat()
			};
		}

		public static Vector2i GetChunkPosition(this Vector2 vector)
		{
			return new Vector2i
			{
				X = (int) vector.X/(int) Chunk.ChunkSize,
				Y = (int) vector.Y/(int) Chunk.ChunkSize
			};
		}
	}
}