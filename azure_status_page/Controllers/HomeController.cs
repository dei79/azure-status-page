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

		[HttpGet]
		public ActionResult Index()
		{
			// load the configuration
			var config = configService.LoadConfiguration();

			// render the config
			return View(config);
		}

		[HttpPost]
		public ActionResult Index(MeterStorageConfigurationModel config)
		{
			// store the config
			if (!configService.VerifyAndStoreConfiguration(config))
				ViewBag.Error = "Failed to store the configuration";
			else
				ViewBag.Success = "Stored the configuration successfully";

			// render the view again
			return View(config);
		}

		[HttpPost]
		public ActionResult Install()
		{
			// install the webjob
			var webJobInstaller = new AzureWebJobInstaller("CheckMetersAndUpdateSite", Path.Combine(Server.MapPath("/"), "App_Data", "WebJob"), AzureEnvironmentService.WebSiteLocation);
			webJobInstaller.InstallOrUpdateWebJob();

			// render the view again
			return RedirectToAction("Index");
		}

	}
}
