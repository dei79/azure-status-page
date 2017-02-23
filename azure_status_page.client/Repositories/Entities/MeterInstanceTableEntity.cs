using System;
using azure_status_page.client.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace azure_status_page.client.Repositories.Entities
{
	internal class MeterInstanceTableEntity : TableEntity
	{
		public MeterInstanceTableEntity(string MeterId, string MeterInstanceId)
		{
			this.PartitionKey = MeterId;
			this.RowKey = MeterInstanceId;
			this.MeterId = MeterId;
			this.MeterInstanceId = MeterInstanceId;
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

		public static MeterInstanceTableEntity FromModel(MeterInstance instance)
		{
			return new MeterInstanceTableEntity(instance.MeterId, instance.MeterInstanceId)
			{
				MeterName = instance.MeterName,
				MeterCategory = instance.MeterCategory,
				MeterType = Convert.ToDouble(Convert.ToInt32(instance.MeterType)),
				MeterValue = Convert.ToDouble(instance.MeterValue),
				MeterInstanceValue = Convert.ToDouble(instance.MeterInstanceValue),
				MeterInstanceTimestamp = instance.MeterInstanceTimestamp.ToString("yyyy-MM-ddTHH:mm:ssZ")
			};
		}
	}
}
