using System;
using azure_status_page.client.Models;

namespace azure_status_page.core
{
	public class EqualsValueCheckMeterProvider : ICheckMeterProvider
	{
		public MeterCheckResult checkMeterInstance(MeterInstance instance)
		{
			return new MeterCheckResult(instance, (instance.MeterInstanceValue.Equals(instance.MeterValue)));
		}
	}
}