using System;
using System.IO;

namespace SquareCubed.Utils.Logging
{
	public class Logger
	{
		private readonly string _tag;
		private StreamWriter _logWriter;

		public Logger(string tag, FileStream log = null)
		{
			_tag = tag;
			_logWriter = log == null ? null : new StreamWriter(log);
		}

		public void LogInfo(string format, params string[] args)
		{
			string text = string.Format(format, args);
			LogInfo(text);
		}

		public void LogInfo(string text)
		{
			// Build String
			string writeText = string.Format("{0, -8}| {1}", _tag, text);

			// Write to Console and File
			Console.WriteLine(writeText);
			if(_logWriter != null)
				_logWriter.WriteLine(writeText);
		}
	}
}
