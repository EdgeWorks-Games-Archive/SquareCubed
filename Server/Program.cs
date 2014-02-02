namespace Server
{
	static class Program
	{
		static void Main()
		{
			var server = new SquareCubed.Server.Server();
			server.Run();
		}
	}
}