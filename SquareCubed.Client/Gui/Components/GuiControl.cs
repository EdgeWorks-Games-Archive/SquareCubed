using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Gui.Components
{
	public abstract class GuiControl
	{
		private GuiParentControl _parent;

		public GuiParentControl Parent
		{
			get { return _parent; }
			set
			{
				// If already this, don't do anything
				if (_parent == value) return;

				// If new parent isn't nothing
				if (value != null)
				{
					// Tell the new parent we need to be added
					// Adding will remove the old parent and set the new one
					value.Controls.Add(this);
				}
				else
				{
					// Tell the old parent we need to be removed
					_parent.Controls.Remove(this);
				}
			}
		}

		public sealed class GuiControlCollection : IEnumerable<GuiControl>
		{
			private readonly List<GuiControl> _children = new List<GuiControl>();
			private readonly GuiParentControl _owner;

			public GuiControlCollection(GuiParentControl owner)
			{
				_owner = owner;
			}

			public IEnumerator<GuiControl> GetEnumerator()
			{
				return _children.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void Add(GuiControl child)
			{
				Contract.Requires<ArgumentNullException>(child != null);

				// If it already has a parent, remove it
				if (child._parent != null)
					child._parent.Controls.Remove(child);

				// Add the child and set its parent
				_children.Add(child);
				child._parent = _owner;
			}

			public void Remove(GuiControl child)
			{
				Contract.Requires<ArgumentNullException>(child != null);

				// Remove the child and reset its parent
				_children.Remove(child);
				child._parent = null;
			}
		}
	}

	public abstract class GuiParentControl : GuiControl
	{
		protected GuiParentControl()
		{
			Controls = new GuiControlCollection(this);
		}

		public GuiControlCollection Controls { get; private set; }
	}
}