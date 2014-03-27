using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareCubed.Client.Graphics
{
	public class ShaderUniform
	{
		private readonly int _location;

		public ShaderUniform(int location)
		{
			_location = location;
		}
	}
}
