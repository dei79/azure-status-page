using System;
using System.IO;

namespace azure_status_page.core
{
	public delegate void ShutdownEventHandler(object sender);

	public class AzureWebJobShutdownService : IDisposable
	{
		private FileSystemWatcher fileSystemWatcher { get; set; }
		private string shutdownFile { get; set; }

		public event ShutdownEventHandler OnShutdown;

		public AzureWebJobShutdownService()
		{
			// Get the shutdown file path from the environment
			shutdownFile = Environment.GetEnvironmentVariable("WEBJOBS_SHUTDOWN_FILE");
			if (shutdownFile == null) { shutdownFile = "/tmp/stop"; }

			// Setup a file system watcher on that file's directory to know when the file is created
			fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(shutdownFile));
			fileSystemWatcher.Created += OnChanged;
			fileSystemWatcher.Changed += OnChanged;
			fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;
			fileSystemWatcher.IncludeSubdirectories = false;
			fileSystemWatcher.EnableRaisingEvents = true;
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
		}

		public void Dispose()
		{
			fileSystemWatcher.EnableRaisingEvents = false;
			OnShutdown = null;
		}
	}
}
