using OpenTK;
using SquareCubed.Server.Structures;

namespace SquareCubed.Server.Players
{
	public struct SpawnPoint
	{
		public Vector2 Position { get; set; }
		public Structure Structure { get; set; }
	}
}
