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

		public Point Position { get; set; }
		public abstract Size Size { get; set; }

		public GuiParentControl Parent
		{
			get { return _parent.Property; }
			set { _parent.Property = value; }
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~GuiControl()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool managed)
		{
		}

		public abstract void Render();

		public abstract class GuiParentControl : GuiControl
		{
			protected GuiParentControl()
			{
				Controls = new ParentLink.ChildrenCollection(this, c => c._parent);
			}

			public ParentLink.ChildrenCollection Controls { get; private set; }

			public override void Render()
			{
				foreach (var control in Controls)
				{
					control.Render();
				}
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