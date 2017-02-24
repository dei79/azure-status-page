using System;
using azure_status_page.client.Models;

namespace azure_status_page.core
{
	public class MinValueCheckMeterProvider : ICheckMeterProvider
	{
		public MeterCheckResult checkMeterInstance(MeterInstance instance)
		{
			return new MeterCheckResult(instance, (instance.MeterInstanceValue > instance.MeterValue));
		}
	}
}
