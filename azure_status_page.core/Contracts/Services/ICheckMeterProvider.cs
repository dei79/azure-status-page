using System;
namespace azure_status_page.core
{
	public interface ICheckMeterProvider
	{		
		MeterCheckResult checkMeterInstance(MeterInstance instance);
	}
}
