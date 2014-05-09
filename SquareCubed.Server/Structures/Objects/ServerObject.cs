using OpenTK;

namespace SquareCubed.Server.Structures.Objects
{
	public class ServerObject
	{
		/// <summary>Position of the object relative to the chunk.</summary>
		public Vector2 Position { get; set; }

		// TODO: Change to be auto resolved
		public int Id { get; set; }
	}
}
