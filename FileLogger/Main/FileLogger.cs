using FileLogger.Models;
using Microsoft.Extensions.Logging;

namespace FileLogger.Main
{
	public sealed class FileLogger(string categoryName, Func<FileLoggerOptions> getOptions) : ILogger
	{
		private readonly static Lock _lock = new();

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		{
			return default!;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel != LogLevel.None && File.Exists(getOptions().LogFilePath);
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}

			string message = formatter(state, exception);

			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			message = $"{DateTime.UtcNow:G} [{categoryName}:{logLevel}:{eventId.Id}]: {message}" + Environment.NewLine;

			if (exception != null)
			{
				message += exception + Environment.NewLine;
			}

			lock (_lock)
			{
				File.AppendAllText(getOptions().LogFilePath, message);
			}
		}
	}
}
