using Lidgren.Network;
using OpenTK;

namespace SquareCubed.Data
{
	public static class VectorExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Vector2 vector)
		{
			msg.Write(vector.X);
			msg.Write(vector.Y);
		}

		public static Vector2 ReadVector2(this NetIncomingMessage msg)
		{
			return new Vector2
			{
				X = msg.ReadFloat(),
				Y = msg.ReadFloat()
			};
		}
	}
}
