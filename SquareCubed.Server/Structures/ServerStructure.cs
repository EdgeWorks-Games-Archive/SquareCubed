using System;
using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Common.Data;
using SquareCubed.Common.Utils;
using SquareCubed.Server.Structures.Objects;
using SquareCubed.Server.Units;
using World = SquareCubed.Server.Worlds.World;

namespace SquareCubed.Server.Structures
{
	public class ServerStructure
	{
		private readonly List<Unit> _units = new List<Unit>();

		public ServerStructure(Vector2 tempCenter)
		{
			Chunks = new List<ServerChunk>();
			Objects = new List<ServerObjectBase>();
			WorldLink = new ParentLink<World, ServerStructure>(this, w => w.Structures);
			WorldLink.ParentSet += (s, e) =>
			{
				Body = new Body(e.Parent.Physics)
				{
					BodyType = BodyType.Dynamic,
					AngularDamping = 0.5f
				};
				var vertices = new Vertices
				{
					new Microsoft.Xna.Framework.Vector2(tempCenter.X - 1, tempCenter.Y + 1), // Left Top
					new Microsoft.Xna.Framework.Vector2(tempCenter.X - 1, tempCenter.Y - 1), // Left Bottom
					new Microsoft.Xna.Framework.Vector2(tempCenter.X + 1, tempCenter.Y - 1), // Right Bottom
					new Microsoft.Xna.Framework.Vector2(tempCenter.X + 1, tempCenter.Y + 1), // Right Top
				};
				var shape = new PolygonShape(vertices, 1.0f);
				Body.CreateFixture(shape);
			};
			WorldLink.ParentRemove += (s, e) =>
			{
				Body.Dispose();
				Body = null;
			};

			Units = new ParentLink<ServerStructure, Unit>.ChildrenCollection(this, u => u.StructureLink);
		}

		[CLSCompliant(false)]
		public Body Body { get; private set; }

		public Vector2 Position
		{
			get { return new Vector2(Body.Position.X, Body.Position.Y); }
			set { Body.Position = new Microsoft.Xna.Framework.Vector2(value.X, value.Y); }
		}

		public Vector2 Force { get; set; }
		public float Torque { get; set; }

		public Vector2 LocalCenter
		{
			get { return new Vector2(Body.LocalCenter.X, Body.LocalCenter.Y); }
			set { Body.LocalCenter = new Microsoft.Xna.Framework.Vector2(value.X, value.Y); }
		}

		public ParentLink<World, ServerStructure> WorldLink { get; private set; }

		public World World
		{
			get { return WorldLink.Property; }
			set { WorldLink.Property = value; }
		}

		public int Id { get; set; }
		public List<ServerChunk> Chunks { get; set; }
		public List<ServerObjectBase> Objects { get; set; }

		public ParentLink<ServerStructure, Unit>.ChildrenCollection Units { get; private set; }

		public void RegenerateShapes()
		{
		}

		// TODO: This basically should just be done through the Objects property, I'm leaving this while I'm refactoring for now
		public void AddObject(float x, float y, int id, TypeRegistry<IServerObjectType> types)
		{
			Debug.Assert(types != null);
			Debug.Assert(id >= 0);
			Debug.Assert(id <= 20);

			var obj = types.GetType(id).CreateNew(this);
			obj.Position = new Vector2(x, y);
			Objects.Add(obj);
		}

		internal void ApplyForces()
		{
			Body.ApplyTorque(Torque);
			Body.ApplyForce(new Microsoft.Xna.Framework.Vector2(Force.X, Force.Y), Body.WorldCenter);
		}
	}

	public static class StructureExtensions
	{
		public static void Write(this NetOutgoingMessage msg, ServerStructure structure, TypeRegistry<IServerObjectType> types)
		{
			Debug.Assert(msg != null);
			Debug.Assert(structure != null);

			// Add metadata and position
			msg.Write(structure.Id);
			msg.Write(structure.Position);
			/*msg.Write(structure.Force);
			msg.Write(structure.Torque);*/
			msg.Write(structure.Body.Rotation);
			msg.Write(structure.LocalCenter);

			// Add structure chunk data
			msg.Write(structure.Chunks.Count);
			foreach (var chunk in structure.Chunks)
				msg.Write(chunk);

			// Write all the objects to the message
			msg.Write(structure.Objects.Count);
			foreach (var obj in structure.Objects)
			{
				msg.Write(types.GetId(obj.Type));
				msg.Write(obj.Id);
				msg.Write(obj.Position);
			}
		}
	}
}