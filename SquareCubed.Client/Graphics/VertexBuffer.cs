using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Graphics
{
	internal sealed class VertexBuffer : IDisposable
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
				new IntPtr(sizeof (float)*data.Length), data,
				BufferUsageHint.StaticDraw);

			// Clean up
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		~VertexBuffer()
		{
			Dispose();
		}

		public void Dispose()
		{
			GL.DeleteBuffer(_vertexBuffer);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///     Binds the buffer and creates a new lifetime
		///     object to unbind the buffer once done.
		/// </summary>
		/// <returns>A new buffer lifetime object that should be disposed when done.</returns>
		public ActivationLifetime Activate()
		{
			return new ActivationLifetime(_vertexBuffer);
		}

		public sealed class ActivationLifetime : IDisposable
		{
			public ActivationLifetime(int vertexBuffer)
			{
				GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			}

			public void Dispose()
			{
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			}
		}
	}
}