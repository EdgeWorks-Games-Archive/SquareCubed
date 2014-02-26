using System;
using System.Security.Cryptography.X509Certificates;

namespace SquareCubed.Client.Structures
{
	public class Structures
	{
		private readonly StructuresNetwork _network;

		public Structures(Client client)
		{
			_network = new StructuresNetwork(client, this);
		}

		public void OnStructureData(Structure structure)
		{
			Console.WriteLine("Received structure!");
		}
	}
}