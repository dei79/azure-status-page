using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using azure_status_page.core;

namespace azure_status_page.Controllers
{
	public class HomeController : Controller
	{
		private SiteExtensionConfigurationService configService = new SiteExtensionConfigurationService();
		private AzureWebJobInstaller webJobInstaller = new AzureWebJobInstaller("CheckMetersAndUpdateSite", Path.Combine(AzureEnvironmentService.SiteExtensionLocation, "App_Data", "WebJob"), AzureEnvironmentService.WebSiteLocation);

		private void PrepareViewBag()
		{
			// check if the webjob is installed 
			ViewBag.WebJobInstalled = webJobInstaller.IsWebJobInstalled();
		}

		[HttpGet]
		public ActionResult Index()
		{
			// load the configuration
			var config = configService.LoadConfiguration();

			// prepare our default view bag
			PrepareViewBag();

			// render the config
			return View(config);
		}

		[HttpPost]
		public ActionResult Index(MeterStorageConfigurationModel config)
		{
			// store the config
			configService.VerifyAndStoreConfiguration(config);

			// prepare our default view bag
			PrepareViewBag();

			// render the config
			return View(config);
		}

		[HttpPost]
		public ActionResult Install()
		{
			// install the webjob
			webJobInstaller.InstallOrUpdateWebJob();

			// render the config
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Generate()
		{
			// prepare our default view bag
			PrepareViewBag();

			// render the config
			return RedirectToAction("Index");
		}

	}
}
