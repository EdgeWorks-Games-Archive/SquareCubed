using System;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Graphics
{
	class VertexBuffer
	{
		private readonly int _vertexBuffer;

		public VertexBuffer(float[] data)
		{
			// Create the new buffer
			_vertexBuffer = GL.GenBuffer();

			// Bind it and set the data
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(
				BufferTarget.ArrayBuffer,
                new IntPtr(sizeof(float) * data.Length), data,
				BufferUsageHint.StaticDraw);

			// Clean up
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}
}
