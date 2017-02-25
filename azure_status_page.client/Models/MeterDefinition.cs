using System;
namespace azure_status_page.client.Models
{
	public enum nMeterTypes
	{
		Heartbeat = 1,
		MinValue = 2,
		MaxValue = 3,
		EqualsValue = 4
	}

	public class MeterDefinition
	{
		public String MeterId { get; set; }
		public String MeterName { get; set; }
		public String MeterCategory { get; set; }
		public nMeterTypes MeterType { get; set; }
		public Decimal MeterValue { get; set; }
		public Int32 MeterDisplayOrder { get; set; }

		public MeterDefinition() { }

		public MeterDefinition(MeterDefinition src)
		{
			MeterId = src.MeterId;
			MeterName = src.MeterName;
			MeterCategory = src.MeterCategory;
			MeterType = src.MeterType;
			MeterValue = src.MeterValue;
			MeterDisplayOrder = src.MeterDisplayOrder;
		}
	}
}
