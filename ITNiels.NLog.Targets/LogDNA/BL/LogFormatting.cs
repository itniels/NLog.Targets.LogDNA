using System;
using System.Linq;
using Newtonsoft.Json;

namespace NLog.Targets.LogDNA
{
	public static class LogFormatting
	{
		/// <summary>
		/// JSON formats the object into a string
		/// </summary>
		public static string FormatParams(object[] param)
		{
			// Return an empty string if there are no params
			if (param == null || param.Length == 0)
				return string.Empty;

			// Parse the object based on single or multiple params.
			// This will make it look a bit better in the log output.
			if (param.Length == 1)
				return $" | {JsonConvert.SerializeObject(param.SingleOrDefault())}";
			else
				return $" | {JsonConvert.SerializeObject(param)}";
		}

		/// <summary>
		/// Formats an exception
		/// </summary>
		public static string FormatException(Exception exception)
		{
			if (exception == null)
				return string.Empty;

			return $" | {JsonConvert.SerializeObject(exception)}";
		}
	}
}
