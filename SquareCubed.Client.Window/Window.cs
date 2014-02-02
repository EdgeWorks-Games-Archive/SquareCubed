using OpenTK;
using OpenTK.Graphics;

namespace SquareCubed.Client.Window
{
	/// <summary>
	///     Representation of the game window.
	///     Not unit tested because we don't want unit tests to set up an actual window.
	/// </summary>
	public class Window : GameWindow
	{
		public Window()
			: base(800, 600, new GraphicsMode(32, 0, 0, 4))
		{
		}
	}
}