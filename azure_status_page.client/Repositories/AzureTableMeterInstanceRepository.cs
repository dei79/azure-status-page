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
			// query
			var queryResult = LoadInstances<MeterInstanceTableEntity>(StorageTable);

			// Print the fields for each customer.
			var result = new List<MeterInstance>();
			foreach (MeterInstanceTableEntity entity in queryResult)
			{
				result.Add(entity.ToModel());
			}

			// done
			return result;
		}

		public void StoreInstance(MeterInstance instance)
		{
			// Generate the table store entity
			var entity = MeterInstanceTableEntity.FromModel(instance);

			// Merge
			InsertOrMerge(entity, StorageTable);
		}
	}
}
