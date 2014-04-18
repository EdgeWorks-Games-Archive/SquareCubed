using System;
using OpenTK.Input;

namespace SquareCubed.Common.Data
{
	public static class KeyExtensions
	{
		/// <summary>
		///     Converts OpenTK Keys to Virtual Key codes.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static int ToVkCode(this Key key)
		{
			// Convert the alphanumeric keys
			if (key >= Key.A && key <= Key.Z)
				return (int) key - (0x41 - (int) Key.A);
			if (key >= Key.Number0 && key <= Key.Number9)
				return (int) key - (0x30 - (int) Key.Number0);
			if (key >= Key.F1 && key <= Key.F35)
				return (int) key - (0x70 - (int) Key.F1);

			// Convert any misc keys
			switch (key)
			{
				case Key.Back:
					return 0x08;
				case Key.Tab:
					return 0x09;
				case Key.Enter:
					return 0x0D;
				case Key.Pause:
					return 0x13;
				case Key.CapsLock:
					return 0x14;
				case Key.Space:
					return 0x20;

				case Key.Left:
					return 0x25;
				case Key.Up:
					return 0x26;
				case Key.Right:
					return 0x27;
				case Key.Down:
					return 0x28;

				case Key.Delete:
					return 0x2E;
				case Key.WinLeft:
					return 0x5B;
				case Key.WinRight:
					return 0x5C;

				case Key.KeypadMultiply:
					return 0x6A;
				case Key.KeypadAdd:
					return 0x6B;
				case Key.KeypadSubtract:
					return 0x6D;
				case Key.KeypadPeriod:
					return 0x6E;

				case Key.ShiftLeft:
					return 0xA0;
				case Key.ShiftRight:
					return 0xA1;
				case Key.ControlLeft:
					return 0xA2;
				case Key.ControlRight:
					return 0xA3;
				case Key.AltLeft:
					return 0xA4;
				case Key.AltRight:
					return 0xA5;

				case Key.Semicolon:
					return 0xBA;
				case Key.Plus:
					return 0xBB;
				case Key.Comma:
					return 0xBC;
				case Key.Minus:
					return 0xBD;
				case Key.Period:
					return 0xBE;
				case Key.Slash:
					return 0xBF;
				case Key.Tilde:
					return 0xC0;
				case Key.BracketLeft:
					return 0xDB;
				case Key.BackSlash:
					return 0xDC;
				case Key.BracketRight:
					return 0xDD;
				case Key.Quote:
					return 0xDE;

				default:
#if DEBUG
					// In debug we want to know if this happens
					throw new Exception("Could not convert key to vk code!");
#else
	// In release we rather just turn it into a space
					return 0x20;
#endif
			}
		}
	}
}