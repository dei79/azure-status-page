using System;
using System.Collections.Generic;

namespace azure_status_page.core
{
	public class SiteReplacementStatusParameters
	{
		public String StatusClass { get; set; }
		public String StatusMessage { get; set; }
		public String StatusUpdate { get; set; } 
	}

	public class ServiceStatusParameters : SiteReplacementStatusParameters
	{
		public String ServiceName { get; set; }
	}

	public class SiteReplacementParameters
	{
		public String Title { get; set; }
		public String BrandIcon { get; set; }
		public String FavIcon { get; set; }
		public SiteReplacementStatusParameters Status { get; set; } = new SiteReplacementStatusParameters();
		public List<ServiceStatusParameters> Services { get; set; } = new List<ServiceStatusParameters>();

	}
}
