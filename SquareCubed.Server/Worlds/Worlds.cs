using SquareCubed.Utils;

namespace SquareCubed.Server.Worlds
{
	public class Worlds
	{
		public AutoDictionary<World> WorldList { get; private set; }
		public World TestWorld { get; private set; }

		public Worlds(Server server)
		{
			WorldList = new AutoDictionary<World>();

			// Add default test world
			TestWorld = new World(server);
			WorldList.Add(TestWorld);
		}
	}
}
