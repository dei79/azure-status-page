using System;
using System.IO;

namespace azure_status_page.core
{
	public delegate void ShutdownEventHandler(object sender);

	public class AzureWebJobShutdownService : IDisposable
	{
		private FileSystemWatcher fileSystemWatcher { get; set; }
		private string shutdownFile { get; set; }
		private string peekFile { get; set; }

		public event ShutdownEventHandler OnShutdown;
		public event ShutdownEventHandler OnPeek;

		public AzureWebJobShutdownService()
		{
			// Get the shutdown file path from the environment
			shutdownFile = AzureWebJobShutdownService.GetShutdownFileName();

			// get the peek file 
			peekFile = AzureWebJobShutdownService.GetPeekFileName();

			// Setup a file system watcher on that file's directory to know when the file is created
			fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(shutdownFile));
			fileSystemWatcher.Created += OnChanged;
			fileSystemWatcher.Changed += OnChanged;
			fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;
			fileSystemWatcher.IncludeSubdirectories = false;
			fileSystemWatcher.EnableRaisingEvents = true;
		}

		private static string GetShutdownFileName()
		{
			var triggerFileName = Environment.GetEnvironmentVariable("WEBJOBS_SHUTDOWN_FILE");
			if (triggerFileName == null) { triggerFileName = "/tmp/stop.notify"; }
			return triggerFileName;
		}

		private static string GetPeekFileName()
		{
			var triggerFileName = AzureWebJobShutdownService.GetShutdownFileName();
			return Path.Combine(Path.GetDirectoryName(triggerFileName), "peek.notify");
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath.IndexOf(Path.GetFileName(shutdownFile), StringComparison.OrdinalIgnoreCase) >= 0)
			{
				if (OnShutdown != null)
				{
					OnShutdown(this);
				}
			}
			else if (e.FullPath.IndexOf(Path.GetFileName(peekFile), StringComparison.OrdinalIgnoreCase) >= 0)
			{
				if (OnPeek != null)
				{
					OnPeek(this);
				}
			}
		}

		public void Dispose()
		{
			fileSystemWatcher.EnableRaisingEvents = false;
			OnShutdown = null;
		}

		public static void Peek()
		{
			var peekFileName = AzureWebJobShutdownService.GetPeekFileName();
			if (File.Exists(peekFileName))
				File.Delete(peekFileName);
				
			File.Create(peekFileName).Dispose();
		}
	}
}
