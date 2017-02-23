using System;
using System.Collections.Generic;
using azure_status_page.client.Contracts.Repositories;
using azure_status_page.client.Models;
using azure_status_page.client.Repositories.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace azure_status_page.client.Repositories
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

		public void StoreInstance(MeterInstance instance)
		{
			// Create the table client.
			CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

			// Retrieve a reference to the table.
			CloudTable table = tableClient.GetTableReference(StorageTable);

			// Create the batch operation.
			TableBatchOperation batchOperation = new TableBatchOperation();

			// Generate the table store entity
			var entity = MeterInstanceTableEntity.FromModel(instance);

			// Add the entity to the batch
			batchOperation.InsertOrMerge(entity);

			// Merge
			table.ExecuteBatch(batchOperation);
		}
	}
}
