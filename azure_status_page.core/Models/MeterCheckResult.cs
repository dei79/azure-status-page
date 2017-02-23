using System;
using azure_status_page.client.Models;

namespace azure_status_page.core
{
	public class MeterCheckResult : MeterInstance
	{
		public Boolean MeterCheckPassed { get; set; }
		public String  MeterCheckAlarmMessage { get; set; }

		public MeterCheckResult(MeterInstance meterInstance, Boolean checkPassed)
			: this(meterInstance, checkPassed, String.Empty)
		{
		}


		public MeterCheckResult(MeterInstance meterInstance, Boolean checkPassed, String checkAlarmMessage)
			:base(meterInstance)
		{
			MeterCheckPassed = checkPassed;
			MeterCheckAlarmMessage = checkAlarmMessage;
		}
	}
}
