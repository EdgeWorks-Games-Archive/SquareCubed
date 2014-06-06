using System;
using System.Diagnostics;
using System.Drawing;
using OpenTK.Input;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Gui.Controls
{
	using ParentLink = ParentLink<GuiControl.GuiParentControl, GuiControl>;

	public abstract partial class GuiControl : IDisposable
	{
		private readonly ParentLink _parent;
		private bool _isFocused;

		protected GuiControl()
		{
			_parent = new ParentLink(this, p => p.Controls);
		}

		public bool IsHovered { get; private set; }
		public bool IsHeld { get; private set; }

		/// <summary>
		///     True if this control is the focused child of the parent control.
		/// </summary>
		public bool IsFocused
		{
			get { return _isFocused; }
			set
			{
				if (value == _isFocused) return;

				if (Parent != null)
					Parent.FocusedChild = value ? this : null;
			}
		}

		public Point Position { get; set; }
		public abstract Size Size { get; set; }

		public Rectangle BoundingBox
		{
			get { return new Rectangle(Position, Size); }
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

		public event EventHandler Click = (s, e) => { };

		internal void HandleKeyChar(char key)
		{
			// The parent sends the key down only to the focused child
			// so we don't have to check that here.
			OnKeyChar(key);
		}

		internal void HandleKeyDown(Key key)
		{
			// The parent sends the key down only to the focused child
			// so we don't have to check that here.
			OnKeyDown(key);
		}

		internal void HandleMouseMove(MouseMoveData data)
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

		internal void HandleMouseDown(MousePressData data)
		{
			if (BoundingBox.Contains(data.Position))
				OnMouseDown(data);
		}

		internal void HandleMouseUp(MousePressData data)
		{
			if (BoundingBox.Contains(data.Position))
				OnMouseUp(data);
		}

		protected virtual void OnKeyChar(char key)
		{
		}

		protected virtual void OnKeyDown(Key key)
		{
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

		protected virtual void OnMouseDown(MousePressData data)
		{
			Debug.Assert(data != null);

			IsHeld = true;
			data.EndEvent.Event += (s, e) => IsHeld = false;
		}

		protected virtual void OnMouseUp(MousePressData data)
		{
			if (IsHeld)
				OnMouseClick(data);
		}

		protected virtual void OnMouseClick(MousePressData data)
		{
			IsFocused = true;
			Click(this, EventArgs.Empty);
		}

		internal abstract void Render(float delta);
	}
}