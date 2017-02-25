using System;
using System.Collections.Generic;
using azure_status_page.client.Models;
using azure_status_page.core.Models;

namespace azure_status_page.ViewModels
{
	public class MetersViewModel
	{
		public List<MeterDefinition> ClientSideMeters { get; set; } = new List<MeterDefinition>();

		public List<MeterDefinitionServerBased> StatusPageMeters { get; set; } = new List<MeterDefinitionServerBased>();
	}
}
