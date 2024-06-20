using System.Text;

namespace ProductAPI
{
	/// <summary>
	/// Local file logging class
	/// </summary>
	public static class Logger
	{
		#region Log File
		const string _logFileRoot = "productApiLog.txt";
		private static string? _today;
		private static string Today
		{
			get
			{
				_today ??= DateTime.Today.ToString("yyyy.MM.dd");
				return _today;
			}
		}

		private static string LogFileDirectory
		{
			get
			{
				var dir = "C:\\Temp";
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);
				return dir;
			}
		}

		private static string Filename => $"{LogFileDirectory}\\{Today}_{_logFileRoot}";

		/// <summary>
		/// Create a verbose log
		/// </summary>
		/// <param name="logString"></param>
		public static void Verbose(string logString) => AppendToLogFile(LogCategory.Verbose, logString);

		/// <summary>
		/// Create an information log
		/// </summary>
		/// <param name="logString"></param>
		public static void Information(string logString) => AppendToLogFile(LogCategory.Information, logString);

		/// <summary>
		/// Create a warning log
		/// </summary>
		/// <param name="logString"></param>
		public static void Warning(string logString) => AppendToLogFile(LogCategory.Warning, logString);

		/// <summary>
		/// Create an error log
		/// </summary>
		/// <param name="logString"></param>
		public static void Error(string logString) => AppendToLogFile(LogCategory.Error, logString);

		/// <summary>
		/// Create an exception log
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="logString"></param>
		/// <param name="details"></param>
		public static void Exception(Exception exception, string logString, params string?[] details) => AppendToLogFile(LogCategory.Exception, logString, $"Exception Message: {exception.Message}", details);

		private static void AppendToLogFile(LogCategory cat, string logString, string? extraString = null, params string?[] details)
		{
			var detailsString = string.Empty;
			if (details != null)
			{
				var sb = new StringBuilder($"{Environment.NewLine}  Details: ");
				var itemCount = 0;
				foreach (var item in details)
				{
					if (item != null && !string.IsNullOrWhiteSpace(item))
					{
						sb.Append($"{item}, ");
						itemCount++;
					}
				}
				if (itemCount > 0)
					detailsString = sb.ToString();
			}

			var msg = $"{DateTime.Now:HH:mm:ss.fff} - {cat.Value}: {logString} - {extraString} {detailsString}{Environment.NewLine}";
			File.AppendAllText(Filename, msg);
			CleanUpOldLogFiles();
		}

		private static void CleanUpOldLogFiles()
		{
			Task.Run(() =>
			{
				var minDate = DateTime.Now.AddDays(-5);
				foreach (var logFile in Directory.GetFiles(LogFileDirectory, $"*{_logFileRoot}"))
				{
					if (File.GetCreationTime(logFile) < minDate)
						try
						{
							File.Delete(logFile);
						}
						catch (Exception) { }
				}
			});
		}
		#endregion

		private class LogCategory
		{
			private LogCategory(string value) => Value = value;

			public string Value { get; set; }

			public static LogCategory Verbose => new("Verbose");
			public static LogCategory Information => new("Information");
			public static LogCategory Warning => new("Warning");
			public static LogCategory Error => new("Error");
			public static LogCategory Exception => new("Exception");
		}
	}
}
