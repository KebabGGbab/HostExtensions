using FileLogger.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace FileLogger.Main
{
	[ProviderAlias("File")]
	public sealed class FileLoggerProvider : ILoggerProvider
	{
		private FileLoggerOptions _options;
		private readonly IDisposable? _onChangeToken;
		private readonly ConcurrentDictionary<string, FileLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

		public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> optionsMonitor)
		{
			_options = optionsMonitor.CurrentValue;
			_onChangeToken = optionsMonitor.OnChange(updatedOptions => _options = updatedOptions);
		}

		public ILogger CreateLogger(string categoryName)
		{
			return _loggers.GetOrAdd(categoryName, name => new FileLogger(name, GetOptions));
		}

		public void Dispose()
		{
			_loggers.Clear();
			_onChangeToken?.Dispose();
		}

		private FileLoggerOptions GetOptions()
		{
			return _options;
		}
	}
}
