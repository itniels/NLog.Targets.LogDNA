using System;
using Newtonsoft.Json;
using NLog.Config;
using NLog.Targets.LogDNA;
using RestSharp;
using RestSharp.Authenticators;

namespace NLog.Targets
{
	[Target("LogDNA")]
	public sealed class LogDnaTarget : TargetWithLayout
	{
		/// <summary>
		/// RestSharp Client used for sending the logs
		/// </summary>
		private RestClient Client;

		/// <summary>
		/// The ingestion key from your LogDNA account
		/// </summary>
		[RequiredParameter]
		public string IngestionKey { get; set; }

		/// <summary>
		/// Name of the app
		/// </summary>
		[RequiredParameter]
		public string Hostname { get; set; }

		/// <summary>
		/// Name of the app
		/// </summary>
		[RequiredParameter]
		public string Appname { get; set; }

		/// <summary>
		/// Name of the environment. <br />
		/// This will be added as a meta field that is searchable. <br />
		/// Useful for adding PRODUCTION and DEVELOPMENT logs seperate.
		/// </summary>
		[DefaultParameter]
		public string Environment { get; set; }

		/// <summary>
		/// Text tags separated by commas eg. 'app,dev,web' <br />
		/// This will be added as a tags field that is searchable. <br />
		/// Useful for sorting logs
		/// </summary>
		[DefaultParameter]
		public string Tags { get; set; }

		/// <summary>
		/// Constructor: Parameterless for using with nlog.config
		/// </summary>
		public LogDnaTarget()	{	}

		/// <summary>
		/// Constructor: For use in code
		/// </summary>
		public LogDnaTarget(string targetName)
		{
			Name = targetName;
		}

		/// <summary>
		/// Constructor: for use in code with required fields
		/// </summary>
		public LogDnaTarget(string targetName, string logDnaIngestionKey, string logDnaHostname, string logDnaAppname, string logDnaEnvironment)
		{
			Name = targetName;

			// Required fields
			IngestionKey = logDnaIngestionKey;
			Hostname = logDnaHostname;
			Appname = logDnaAppname;
			Environment = logDnaEnvironment;
		}

		/// <summary>
		/// Parses a Logevent and sends the line to LogDNA
		/// </summary>
		/// <param name="logEvent"></param>
		protected override void Write(LogEventInfo logEvent)
		{

			// Create client with auth on first request
			if (Client?.Authenticator == null)
			{
				Client = new RestClient($"https://logs.logdna.com");
				Client.Authenticator = new HttpBasicAuthenticator(IngestionKey, "");
			}

			// Prepare Request
			var request = new RestRequest($"/logs/ingest?hostname={Hostname}", Method.POST, DataFormat.Json)
				.AddJsonBody(JsonConvert.SerializeObject(new LogSet(new LogLine {
					Timestamp = ((DateTimeOffset)logEvent.TimeStamp).ToUnixTimeMilliseconds(),
					Line = $"{logEvent.FormattedMessage}{LogFormatting.FormatParams(logEvent.Parameters)}{LogFormatting.FormatException(logEvent.Exception)}",
					App = Appname,
					Level = logEvent.Level.ToString(),
					Environment = Environment,
					Tags = Tags,
					Meta = new {
						LoggerName = logEvent.LoggerName
					}
				})
			));

			// Send log request
			Client.Execute(request);
		}
	}
}
