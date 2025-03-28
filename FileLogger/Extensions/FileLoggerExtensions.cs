using FileLogger.Main;
using FileLogger.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace FileLogger.Extensions
{
	public static class FileLoggerExtensions
	{
		public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
		{
			builder.AddConfiguration();

			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());
			builder.Services.TryAddSingleton<LogFileGeneration>();
			LoggerProviderOptions.RegisterProviderOptions<FileLoggerOptions, FileLoggerProvider>(builder.Services);

			return builder;
		}

		public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLoggerOptions> options)
		{
			builder.Services.Configure(options);
			builder.AddFile();

			return builder;
		}
	}
}
