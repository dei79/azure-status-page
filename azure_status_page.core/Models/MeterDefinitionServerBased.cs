using System;
using azure_status_page.client.Models;

namespace azure_status_page.core.Models
{	
	public class MeterDefinitionServerBased : MeterDefinition
	{
		public string MeterServerCheckType { get; set; }
		public Int32 MeterServerCheckInterval { get; set; }
		public string MeterServerCheckInformation { get; set; }
	}
}
