using System;
using System.Diagnostics;
using Lidgren.Network;
using OpenTK;

namespace SquareCubed.Common.Data
{
	public static class VectorExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Vector2 vector)
		{
			Debug.Assert(msg != null);
			Debug.Assert(vector != null);

			msg.Write(vector.X);
			msg.Write(vector.Y);
		}

		public static Vector2 ReadVector2(this NetIncomingMessage msg)
		{
			Debug.Assert(msg != null);

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