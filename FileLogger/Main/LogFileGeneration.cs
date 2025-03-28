using FileLogger.Models;
using Microsoft.Extensions.Options;

namespace FileLogger.Main
{
	internal sealed class LogFileGeneration : IDisposable
	{
		private readonly IDisposable? _onChangeToken;
		private FileLoggerOptions _options;
		private Timer? _timer;

		public LogFileGeneration(IOptionsMonitor<FileLoggerOptions> options)
		{
			_options = options.CurrentValue;

			_onChangeToken = options.OnChange(updateOptions =>
			{
				_options = updateOptions;

				if (updateOptions.Generation != null)
				{
					InitializeTimer();
				}
				else
				{
					_timer?.Dispose();
				}
			});
		}

		public void InitializeTimer()
		{
			_timer = new Timer(
				callback: new TimerCallback(_options?.Generation?.Rule),
				state: _options?.LogFilePath,
				dueTime: 0,
				period: _options.Generation.DelayCheck);
		}

		public void Dispose()
		{
			_onChangeToken?.Dispose();
			_timer?.Dispose();
		}
	}
}
