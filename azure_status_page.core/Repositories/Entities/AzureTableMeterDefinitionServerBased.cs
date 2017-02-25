using System;
using azure_status_page.client.Models;
using azure_status_page.core.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace azure_status_page.core
{
	public class AzureTableMeterDefinitionServerBased : TableEntity
	{
		public String MeterId { get; set; }
		public String MeterName { get; set; }

		public String MeterCategory { get; set; }
		public Int32 MeterDisplayOrder { get; set; }
		public Double MeterType { get; set; }
		public Double MeterValue { get; set; }

		public string MeterServerCheckType { get; set; }
		public Int32 MeterServerCheckInterval { get; set; }
		public string MeterServerCheckInformation { get; set; }

		public AzureTableMeterDefinitionServerBased(string meterId, string meterServerCheckType)
		{
			this.PartitionKey = meterId;
			this.RowKey = meterServerCheckType;
			this.MeterId = meterId;
			this.MeterServerCheckType = meterServerCheckType;				
		}

		public AzureTableMeterDefinitionServerBased() 
		{ }

		public MeterDefinitionServerBased ToModel() 
		{
			return new MeterDefinitionServerBased()
			{
				MeterId = this.MeterId,
				MeterName = this.MeterName,
				MeterCategory = this.MeterCategory,
				MeterDisplayOrder = this.MeterDisplayOrder,
				MeterType = (nMeterTypes)Enum.ToObject(typeof(nMeterTypes), Convert.ToInt32(this.MeterType)),
				MeterValue = new Decimal(this.MeterValue),
				MeterServerCheckType = this.MeterServerCheckType,
				MeterServerCheckInterval = this.MeterServerCheckInterval,
				MeterServerCheckInformation = this.MeterServerCheckInformation
			};
		}

		public static AzureTableMeterDefinitionServerBased FromModel(MeterDefinitionServerBased model)
		{
			return new AzureTableMeterDefinitionServerBased(model.MeterId, model.MeterServerCheckType)
			{
				MeterId = model.MeterId,
				MeterName = model.MeterName,
				MeterCategory = model.MeterCategory,
				MeterDisplayOrder = model.MeterDisplayOrder,
				MeterType = Convert.ToDouble(Convert.ToInt32(model.MeterType)),
				MeterValue = Convert.ToDouble(model.MeterValue),
				MeterServerCheckType = model.MeterServerCheckType,
				MeterServerCheckInterval = model.MeterServerCheckInterval,
				MeterServerCheckInformation = model.MeterServerCheckInformation
			};
		}
	}
}
