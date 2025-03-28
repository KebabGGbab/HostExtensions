using System.ComponentModel.DataAnnotations;

namespace FileLogger.Models
{
	public sealed class FileLoggerOptions
	{
		[Required, RegularExpression(@"^.*\.txt$", ErrorMessage = "The file path should lead to a file with a .txt extension.")]
		public required string LogFilePath { get; set; }

		public bool CreateIfMissing { get; set; }

		public LogFileGenerationOptions? Generation { get; set; }
	}
}
