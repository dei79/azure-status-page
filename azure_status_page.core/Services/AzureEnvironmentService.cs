using System;
using System.IO;
using System.Reflection;

namespace azure_status_page.core
{
	public static class AzureEnvironmentService
	{		
		public static string DataDirectory { get; private set; }
		public static string WebSiteLocation { get; private set; }
		public static string SiteExtensionLocation { get; private set; }

		public static void Initialize()
		{
			// fall back for local development environment (currently in my MAC)
			DataDirectory = "/tmp/StatusPage";
			WebSiteLocation = "/tmp/WebSiteLocation";
			SiteExtensionLocation = "/Users/dei79/Documents/Projects/azurecosts/azure_status_page/azure_status_page";

			// get the correct path
			if (Environment.GetEnvironmentVariable("home") != null)
			{
				DataDirectory = Environment.ExpandEnvironmentVariables(@"%HOME%\data\StatusPage");
				WebSiteLocation = Environment.ExpandEnvironmentVariables(@"%HOME%\site\wwwroot");
				SiteExtensionLocation = Environment.ExpandEnvironmentVariables(@"%HOME%\SiteExtensions\statuspage");
			}

			// check if DataDirectory exists
			if (!Directory.Exists(DataDirectory))
				Directory.CreateDirectory(DataDirectory);

			// check if WebSiteLocation exists
			if (!Directory.Exists(WebSiteLocation))
				Directory.CreateDirectory(WebSiteLocation);			
		}
	}
}
