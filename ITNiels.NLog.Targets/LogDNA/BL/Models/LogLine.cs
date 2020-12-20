using Newtonsoft.Json;

namespace NLog.Targets.LogDNA
{
	internal class LogLine
	{
		[JsonProperty("line")]
		public string Line { get; set; }

		[JsonProperty("timestamp")]
		public long Timestamp { get; set; }

		[JsonProperty("app")]
		public string App { get; set; }

		[JsonProperty("level")]
		public string Level { get; set; }

		[JsonProperty("env")]
		public object Environment { get; set; }

		[JsonProperty("meta")]
		public object Meta { get; set; }

		[JsonProperty("tags")]
		public string Tags { get; set; }
	}
}
