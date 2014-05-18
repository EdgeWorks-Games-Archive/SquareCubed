using System;
using OpenTK;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class TeleporterObject : IClientObject
	{
		public TeleporterObject(TeleporterObjectType type)
		{
		}

		public int Id { get; set; }
		public Vector2 Position { get; set; }

		public void OnUse()
		{
			Console.WriteLine("Teleport me!");
		}
	}
}