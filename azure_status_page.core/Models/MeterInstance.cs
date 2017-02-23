using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace azure_status_page.core
{
	public class MeterInstance : MeterDefinition
	{
		public string 	MeterInstanceId { get; set; }
		public Decimal 	MeterInstanceValue { get; set; }
		public DateTime MeterInstanceTimestamp { get; set; }

		public MeterInstance() { }

		public MeterInstance(MeterInstance src)
			: base(src)
		{
			MeterInstanceId = src.MeterInstanceId;
			MeterInstanceValue = src.MeterInstanceValue;
			MeterInstanceTimestamp = src.MeterInstanceTimestamp;
		}
	}
}
