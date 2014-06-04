using System;
using System.Drawing;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Gui.Controls
{
	using ParentLink = ParentLink<GuiControl.GuiParentControl, GuiControl>;

	public abstract class GuiControl : IDisposable
	{
		private readonly ParentLink _parent;

		protected GuiControl()
		{
			_parent = new ParentLink(this, p => p.Controls);
		}

		public bool IsHovered { get; private set; }
		public Point Position { get; set; }
		public abstract Size Size { get; set; }

		public Rectangle BoundingBox
		{
			get { return new Rectangle(Position, Size); /*TODO: Add parent position offset*/ }
		}

		public GuiParentControl Parent
		{
			get { return _parent.Property; }
			set { _parent.Property = value; }
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool managed)
		{
		}

		~GuiControl()
		{
			Dispose(false);
		}

		internal virtual void HandleMouseMove(MouseMoveData data)
		{
			if (BoundingBox.Contains(data.Position))
			{
				if (!IsHovered)
					OnMouseEnter(data);
				OnMouseMove(data);
			}
			else if (BoundingBox.Contains(data.PreviousPosition))
			{
				OnMouseExit(data);
				OnMouseMove(data);
			}
		}

		protected virtual void OnMouseMove(MouseMoveData data)
		{
		}

		protected virtual void OnMouseEnter(MouseMoveData data)
		{
			IsHovered = true;
		}

		protected virtual void OnMouseExit(MouseMoveData data)
		{
			IsHovered = false;
		}

		public abstract void Render();

		public abstract class GuiParentControl : GuiControl
		{
			protected GuiParentControl()
			{
				Controls = new ParentLink.ChildrenCollection(this, c => c._parent);
			}

			public ParentLink.ChildrenCollection Controls { get; private set; }
			public abstract Size InternalOffset { get; }

			public override void Render()
			{
				foreach (var control in Controls)
				{
					control.Render();
				}
			}

			protected override void OnMouseMove(MouseMoveData data)
			{
				var internalData = new MouseMoveData(
					new Point(data.Position.X - Position.X - InternalOffset.Width, data.Position.Y - Position.Y - InternalOffset.Height),
					new Point(data.PreviousPosition.X - Position.X - InternalOffset.Width, data.PreviousPosition.Y - Position.Y - InternalOffset.Height));

				foreach (var control in Controls)
					control.HandleMouseMove(internalData);

				base.OnMouseMove(data);
			}

			protected override void Dispose(bool managed)
			{
				if (managed)
				{
					foreach (var control in Controls)
					{
						control.Dispose();
					}
				}

				base.Dispose(managed);
			}
		}
	}
}