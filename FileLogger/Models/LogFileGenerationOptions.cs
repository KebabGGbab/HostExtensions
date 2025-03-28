namespace FileLogger.Models
{
	public sealed class LogFileGenerationOptions
	{
		public int LimitRecord { get; set; } = 100000;

		public int DelayCheck { get; set; } = 60000;

		public bool OverwriteIfExists { get; set; } = false;

		public Action<object> Rule { get; set; }

		public LogFileGenerationOptions()
		{
			Rule = DefaultRule;
		}

		private void DefaultRule(object fileName)
		{
			if (fileName is not string)
			{
				return;
			}

			string newFileName = string.Concat(fileName, "-", Guid.NewGuid().ToString());

			if (!File.Exists(newFileName) || OverwriteIfExists)
			{
				File.Create(newFileName);
			}
			else
			{
				DefaultRule(fileName);
			}
		}
	}
}
