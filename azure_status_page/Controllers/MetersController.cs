using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using azure_status_page.core;
using azure_status_page.core.Models;
using azure_status_page.core.Repositories;
using azure_status_page.ViewModels;

namespace azure_status_page.Controllers
{
    public class MetersController : Controller
    {		
		private SiteExtensionConfigurationService configService = new SiteExtensionConfigurationService();

		private MeterManagerEx initializeMeterManager()
		{
			// try to load the configruation 
			var config = configService.LoadConfiguration();

			// generate the repository
			var repository = new AzureTableMeterManagerExRepository(config.StorageKey, config.StorageSecret, config.StorageTableName);

			// instantiate the meter manager
			return new MeterManagerEx(repository);
		}

		[HttpGet]
        public ActionResult Index()
        {
			// instantiate the meter manager
			var meterManagerEx = initializeMeterManager();

			// build the view model
			var viewModel = new MetersViewModel();

			// get all the MeterDefinitions for the client side meters
			viewModel.ClientSideMeters = meterManagerEx.GetClientSideDefinitions();

			// get all Server Based MeterDefinitions
			viewModel.StatusPageMeters = meterManagerEx.GetServerBasedDefinitions();

			// render the view
			return View (viewModel);
        }

		[HttpGet]
		public ActionResult Add()
		{
			// create the model 
			var meterDefinition = new MeterDefinitionServerBased()
			{
				MeterId = Guid.NewGuid().ToString(),
				MeterType = client.Models.nMeterTypes.Heartbeat,
				MeterServerCheckType = "HTTP",
				MeterServerCheckInformation = String.Empty
			}; 

			return View("Add", meterDefinition);
		}

		[HttpPost]
		public ActionResult Add(MeterDefinitionServerBased model)
		{
			// instantiate the meter manager
			var meterManagerEx = initializeMeterManager();

			// store the meterdefinition to the settings 
			meterManagerEx.CreateOrUpdateServerSideDefinition(model);

			// trigger the webjib
			AzureWebJobShutdownService.Peek();

			// Render the meters
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(String Id) 
		{ 
			// instantiate the meter manager
			var meterManagerEx = initializeMeterManager();

			var meterId = Id.Split('.');
			var meterDefinition = meterManagerEx.GetServerBasedDefinition(meterId[0], meterId[1]);

			// show the dialog
			return View("Add", meterDefinition);
		}

		[HttpGet]
		public ActionResult Delete(String Id)
		{
			// instantiate the meter manager
			var meterManagerEx = initializeMeterManager();

			var meterId = Id.Split('.');
			meterManagerEx.DeleteServerSideDefinition(meterId[0], meterId[1]);

			// Render the meters
			return RedirectToAction("Index");
		}
	}
}
