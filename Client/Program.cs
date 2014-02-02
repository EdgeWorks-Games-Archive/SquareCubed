namespace Client
{
	static class Program
	{
		static void Main()
		{
			using (var client = new SquareCubed.Client.Client())
			{
				client.Run();
			}
		}
	}
}