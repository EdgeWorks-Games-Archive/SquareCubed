using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Gui.Controls
{
	using ParentLink = ParentLink<GuiControl.GuiParentControl, GuiControl>;

	public abstract class GuiControl
	{
		private readonly ParentLink _parent;

		protected GuiControl()
		{
			_parent = new ParentLink(this, p => p.Controls);
		}

		public GuiParentControl Parent
		{
			get { return _parent.Property; }
			set { _parent.Property = value; }
		}

		public abstract class GuiParentControl : GuiControl
		{
			protected GuiParentControl()
			{
				Controls = new ParentLink.ChildrenCollection(this, c => c._parent);
			}

			public ParentLink.ChildrenCollection Controls { get; private set; }
		}
	}
}