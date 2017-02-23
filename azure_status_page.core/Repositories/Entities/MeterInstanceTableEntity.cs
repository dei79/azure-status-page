using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace azure_status_page.core
{
	public class MeterInstanceTableEntity : TableEntity
	{
		public MeterInstanceTableEntity(string MeterId, string MeterInstanceId)
		{
			this.PartitionKey = MeterId;
			this.RowKey = MeterInstanceId;
		}

		public MeterInstanceTableEntity() { }

		public string MeterInstanceId { get; set; }
		public Double MeterInstanceValue { get; set; }
		public String MeterInstanceTimestamp { get; set; }
		public String MeterId { get; set; }
		public String MeterName { get; set; }
		public String MeterCategory { get; set; }
		public Double MeterType { get; set; }
		public Double MeterValue { get; set; }

		public MeterInstance ToModel() {

			return new MeterInstance()
			{
				MeterId = this.MeterId,
				MeterName = this.MeterName,
				MeterCategory = this.MeterCategory,
				MeterType =  (nMeterTypes)Enum.ToObject(typeof(nMeterTypes), Convert.ToInt32(this.MeterType)),
				MeterValue = new Decimal(this.MeterValue),
				MeterInstanceId = this.MeterInstanceId,
				MeterInstanceValue = new Decimal(this.MeterInstanceValue),
				MeterInstanceTimestamp = DateTime.Parse(this.MeterInstanceTimestamp)
			};	
		}
	}
}
