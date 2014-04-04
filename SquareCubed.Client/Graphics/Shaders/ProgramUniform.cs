using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Graphics.Shaders
{
	public class ProgramUniform
	{
		private readonly int _location;

		public ProgramUniform(int location)
		{
			_location = location;
		}

		public void SetInt32(int value)
		{
			GL.Uniform1(_location, value);
		}
	}
}
