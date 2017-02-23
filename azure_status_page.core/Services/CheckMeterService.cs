using System;
using System.Collections.Generic;
using azure_status_page.client;
using azure_status_page.client.Contracts.Repositories;
using azure_status_page.client.Models;
using azure_status_page.client.Repositories;

namespace azure_status_page.core
{
	public class CheckMeterService
	{
		private IMeterInstanceRepository meterInstanceRepository { get; set; }
		private Dictionary<nMeterTypes, ICheckMeterProvider> checkMeterProviders { get; set; }

		public CheckMeterService(MeterStorageConfigurationModel config)
		{
			meterInstanceRepository = new AzureTableMeterInstanceRepository(config.StorageKey, config.StorageSecret, config.StorageTableName);
			checkMeterProviders = new Dictionary<nMeterTypes, ICheckMeterProvider>();
			checkMeterProviders.Add(nMeterTypes.Heartbeat, new HeartbeatCheckMeterProvider());
		}

		public List<MeterCheckResult> Check()
		{
			// get all instances
			var meterInstances = meterInstanceRepository.LoadInstances();

			// check every instance
			var result = new List<MeterCheckResult>();
			foreach(var meterInstance in meterInstances)
			{
				// check if we have a provider
				if (!checkMeterProviders.ContainsKey(meterInstance.MeterType))
				{
					result.Add(new MeterCheckResult(meterInstance, false, "Failed to check the meter because of missing provider"));	
					continue;
				}
										
				// lookup the check provider
				var checkProvider = checkMeterProviders[meterInstance.MeterType];
				result.Add(checkProvider.checkMeterInstance(meterInstance));
			}

			return result;
		}
	}
}
