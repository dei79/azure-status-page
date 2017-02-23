using System;
using azure_status_page.client.Models;

namespace azure_status_page.core
{
	public interface ICheckMeterProvider
	{		
		MeterCheckResult checkMeterInstance(MeterInstance instance);
	}
}
