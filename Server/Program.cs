namespace Server
{
	internal static class Program
	{
		private static void Main()
		{
			var server = new SquareCubed.Server.Server();
			server.Run();
		}
	}
}