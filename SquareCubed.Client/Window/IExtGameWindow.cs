using System;
using OpenTK.Input;
using OpenTK.Platform;

namespace SquareCubed.Client.Window
{
	public interface IExtGameWindow : IGameWindow
	{
		event EventHandler<MouseMoveEventArgs> MouseMove;
		event EventHandler<MouseButtonEventArgs> MouseDown;
		event EventHandler<MouseButtonEventArgs> MouseUp;
	}
}
