using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace azure_status_page.core
{
	public class AzureTableMeterInstanceRepository : AzureTableBaseRepository, IMeterInstanceRepository
	{		
		public AzureTableMeterInstanceRepository(string storageKey, string storageSecret)
			: this(storageKey, storageSecret, "MeterInformation")
		{ }

		public AzureTableMeterInstanceRepository(string storageKey, string storageSecret, string storageTable)
			: base(storageKey, storageSecret, storageTable)
		{ }

		public List<MeterInstance> LoadInstances()
		{
			// Create the table client.
			CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

			// Retrieve a reference to the table.
			CloudTable table = tableClient.GetTableReference(StorageTable);

			// Construct the query operation for all customer entities where PartitionKey="Smith".
			TableQuery<MeterInstanceTableEntity> query = new TableQuery<MeterInstanceTableEntity>();

			// Print the fields for each customer.
			var result = new List<MeterInstance>();
			foreach (MeterInstanceTableEntity entity in table.ExecuteQuery(query))
			{
				result.Add(entity.ToModel());
			}

			// done
			return result;
		}
	}
}
