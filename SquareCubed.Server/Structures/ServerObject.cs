using OpenTK;

namespace SquareCubed.Server.Structures
{
	public class ServerObject
	{
		/// <summary>Position of the object relative to the chunk.</summary>
		public Vector2 Position { get; set; }

		// TODO: Change to be auto resolved (using Type as key in a dictionary?)
		public uint Id { get; set; }
	}
}
