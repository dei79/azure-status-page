using System;
using System.IO;
using System.Reflection;
using System.Threading;
using azure_status_page.core;
using azure_status_page.core.Repositories;

namespace azure_status_page.jobs
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// initialize the environment
			AzureEnvironmentService.Initialize();

			using (var shutdownService = new AzureWebJobShutdownService())
			{
				// generate the waithandle we are using for the innermost loop 
				ManualResetEvent timeoutHandle = new ManualResetEvent(false);
				bool bShutdown = false;

				// configure our shutdown handler
				shutdownService.OnShutdown += (sender) => {					
					Console.WriteLine("Detected Shutdown, interupting the loop");
					bShutdown = true;
					timeoutHandle.Set();
				};

				shutdownService.OnPeek += (sender) => {
					Console.WriteLine("Detected Peek, forcing a loop");
					timeoutHandle.Set();
				};

				// start the inner most loop
				do
				{
					// reset our event
					timeoutHandle.Reset();

					// load the config
					Console.WriteLine("Loading the Azure WebJob Configuration...");
					var config = (new SiteExtensionConfigurationService()).LoadConfiguration();

					// executing the server side checks
					Console.WriteLine("Executing Server Side Checks");
					var meterManagerEx = new MeterManagerEx(config);
					meterManagerEx.ExecuteServerSideChecks();

					// check the meters
					Console.WriteLine("Checking the different Meters");
					var checkMeterService = new CheckMeterService(config);
					var results = checkMeterService.Check();

					// notify if needed
					Console.WriteLine("Notify failed Meters");
					checkMeterService.Notify(results);

					// get the path of the job 
					var jobAssetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Assets");
					var jobSiteTemplate = Path.Combine(jobAssetPath, "default-template.html");

					// update the static status page 
					Console.WriteLine("Updating Status Page");
					var siteGenerator = new StatusPageGeneratorService(Path.Combine(AzureEnvironmentService.WebSiteLocation, "index.html"), jobSiteTemplate);
					siteGenerator.UpdateSite(config.Title, config.Brand, config.FavIcon, results);

					// done
					Console.WriteLine("Waiting for the next cycle (5mins)");

				} while (!timeoutHandle.WaitOne(5 * 60 * 1000) || !bShutdown);
			}
		}
	}
}
