using System.Collections.Generic;
using Newtonsoft.Json;

namespace NLog.Targets.LogDNA
{
	internal class LogSet
	{
		[JsonProperty("lines")]
		public List<LogLine> Lines { get; set; }

		public LogSet(LogLine line)
		{
			Lines = new List<LogLine> { line };
		}
	}
}
