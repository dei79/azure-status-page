using System;
namespace azure_status_page.core
{
	public class HeartbeatCheckMeterProvider : ICheckMeterProvider
	{
		public MeterCheckResult checkMeterInstance(MeterInstance instance)
		{			
			var meterGap = DateTime.Now - instance.MeterInstanceTimestamp;
			if (meterGap.TotalSeconds > Convert.ToDouble(instance.MeterValue))
				return new MeterCheckResult(instance, false, String.Format("Last Heartbeat {0} seconds ago", meterGap.TotalSeconds));
			else
				return new MeterCheckResult(instance, true);			
		}
	}
}
