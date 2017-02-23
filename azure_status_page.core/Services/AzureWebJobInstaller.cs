using System;
using System.IO;

namespace azure_status_page.core
{
	public class AzureWebJobInstaller
	{
		private string WebJobName { get; set; }
		private string SrcDirectory { get; set; }
		private string WebJobDirectory { get; set; }

		public AzureWebJobInstaller(string webJobName, string srcDirectory, string webSiteRoot)
		{			
			WebJobName = webJobName;
			SrcDirectory = srcDirectory;
			WebJobDirectory = Path.Combine(webSiteRoot, "App_Data", "jobs", "continuous", webJobName);
		}


		public Boolean IsWebJobInstalled()
		{
			return Directory.Exists(WebJobDirectory);
		}

		public void InstallOrUpdateWebJob()
		{
			// ensure the WebJobDirectory exists
			if (!Directory.Exists(WebJobDirectory))
				Directory.CreateDirectory(WebJobDirectory);

			// Copy all files from the src directory to the WebJobDirectory
			DirectoryEx.Copy(SrcDirectory, WebJobDirectory, true);
		}
	}
}
