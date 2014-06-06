using System.Drawing;
using OpenTK.Input;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Gui.Controls
{
	using ParentLink = ParentLink<GuiControl.GuiParentControl, GuiControl>;

	public abstract partial class GuiControl
	{
		public abstract class GuiParentControl : GuiControl
		{
			private GuiControl _focusedChild;

			protected GuiParentControl()
			{
				Controls = new ParentLink.ChildrenCollection(this, c => c._parent);
			}

			public ParentLink.ChildrenCollection Controls { get; private set; }
			public abstract Size InternalOffset { get; }

			public GuiControl FocusedChild
			{
				get { return _focusedChild; }
				set
				{
					if (value == _focusedChild) return;

					if (_focusedChild != null)
						_focusedChild._isFocused = false;
					if (value != null)
						value._isFocused = true;

					_focusedChild = value;
				}
			}

			internal override void Render(float delta)
			{
				foreach (var control in Controls)
				{
					control.Render(delta);
				}
			}

			protected override void OnKeyChar(char key)
			{
				if (FocusedChild != null)
					FocusedChild.HandleKeyChar(key);

				base.OnKeyChar(key);
			}

			protected override void OnKeyDown(Key key)
			{
				if (FocusedChild != null)
					FocusedChild.HandleKeyDown(key);

				base.OnKeyDown(key);
			}

			protected override void OnMouseMove(MouseMoveData data)
			{
				var internalData = new MouseMoveData(
					new Point(
						data.Position.X - Position.X - InternalOffset.Width,
						data.Position.Y - Position.Y - InternalOffset.Height),
					new Point(
						data.PreviousPosition.X - Position.X - InternalOffset.Width,
						data.PreviousPosition.Y - Position.Y - InternalOffset.Height));

				foreach (var control in Controls)
					control.HandleMouseMove(internalData);

				base.OnMouseMove(data);
			}

			protected override void OnMouseDown(MousePressData data)
			{
				var internalData = new MousePressData(
					new Point(
						data.Position.X - Position.X - InternalOffset.Width,
						data.Position.Y - Position.Y - InternalOffset.Height),
					data.Button, data.EndEvent);

				foreach (var control in Controls)
					control.HandleMouseDown(internalData);

				base.OnMouseDown(data);
			}

			protected override void OnMouseUp(MousePressData data)
			{
				var internalData = new MousePressData(
					new Point(
						data.Position.X - Position.X - InternalOffset.Width,
						data.Position.Y - Position.Y - InternalOffset.Height),
					data.Button, data.EndEvent);

				foreach (var control in Controls)
					control.HandleMouseUp(internalData);

				base.OnMouseUp(data);
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