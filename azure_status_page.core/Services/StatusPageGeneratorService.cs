using System;
using System.Collections.Generic;
using System.IO;

namespace azure_status_page.core
{
	public class StatusPageGeneratorService
	{
		private string TargetLocation { get; set; }
		private string Template { get; set; }

		public StatusPageGeneratorService(string targetLocation, string template)
		{
			TargetLocation 	= targetLocation;
			Template	 	= template;
		}

		public void UpdateSite(string title, string brandIcon, string favIcon, List<MeterCheckResult> meterCheckResults)
		{
			// generate the basic replacements
			var replacements = new SiteReplacementParameters();
			replacements.Title = title;
			replacements.BrandIcon = brandIcon;
			replacements.FavIcon = favIcon;

			// define our standard overall status 
			replacements.Status.StatusClass = "alert-success";
			replacements.Status.StatusMessage = "All Systems Operational";
			replacements.Status.StatusUpdate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

			// build the services and get the overall status
			var lookUpTable = new Dictionary<string, ServiceStatusParameters>();
			foreach (var meterCheckResult in meterCheckResults)
			{
				// adapt the meter category
				if (meterCheckResult.MeterCategory == null)
					meterCheckResult.MeterCategory = meterCheckResult.MeterName;
				
				// check if we have a status for this category
				if (!lookUpTable.ContainsKey(meterCheckResult.MeterCategory))
				{
					var statTemp = new ServiceStatusParameters() { ServiceName = meterCheckResult.MeterCategory, StatusClass = "success", StatusMessage = "Operational", ServiceOrder = meterCheckResult.MeterDisplayOrder };
					replacements.Services.Add(statTemp);
					lookUpTable.Add(meterCheckResult.MeterCategory, statTemp);
				}

				var stat = lookUpTable[meterCheckResult.MeterCategory];
				if (!meterCheckResult.MeterCheckPassed) { 
					stat.StatusClass = "broken"; 
					stat.StatusMessage = "Service Degradation"; 

					// update overall status
					replacements.Status.StatusClass = "alert-warning";
					replacements.Status.StatusMessage = "Minor service outage";

				};
			}

			// order by display order
			replacements.Services.Sort((ServiceStatusParameters x, ServiceStatusParameters y) => {
				return x.ServiceOrder.CompareTo(y.ServiceOrder);
			});

			// Write the file
			using (TextWriter pageWriter = new StreamWriter(TargetLocation))
			{
				using (var templateReader = new StreamReader(Template))
				{
					var compiledTemplate = HandlebarsDotNet.Handlebars.Compile(templateReader);
					compiledTemplate(pageWriter, replacements);
				}
			}
		}
	}
}
