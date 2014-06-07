namespace Client
{
	internal static class Program
	{
		private static void Main()
		{
			using (var client = new SquareCubed.Client.Client())
			{
				client.Run();
			}
		}
	}
}