using System;
using System.Collections.Generic;
using azure_status_page.client.Contracts.Repositories;
using azure_status_page.client.Models;

namespace azure_status_page.client.Repositories
{
	public class AzureTableMeterManagerRepository : AzureTableBaseRepository, IMeterManagerRepository, IMeterInstanceRepository
	{
		private AzureTableMeterInstanceRepository meterInstanceRepository { get; set; }
			
		public AzureTableMeterManagerRepository(string storageKey, string storageSecret, string storageTable)
			:base(storageKey, storageSecret, storageTable)
		{
			meterInstanceRepository = new AzureTableMeterInstanceRepository(storageKey, storageSecret, storageTable);
		}

		public List<MeterInstance> LoadInstances()
		{
			return meterInstanceRepository.LoadInstances();
		}

		public void StoreInstance(MeterInstance instance)
		{
			meterInstanceRepository.StoreInstance(instance);
		}

		public void DeleteInstance(string MeterId, string MeterInstanceId)
		{
			meterInstanceRepository.DeleteInstance(MeterId, MeterInstanceId);
		}
	}
}
