using System;
using System.Collections.Generic;

namespace azure_status_page.core
{
	public class CheckMeterService
	{
		private IMeterInstanceRepository repository { get; set; }
		private Dictionary<nMeterTypes, ICheckMeterProvider> checkMeterProviders { get; set; }

		public CheckMeterService(MeterStorageConfigurationModel config)
		{
			repository = new AzureTableMeterInstanceRepository(config.StorageKey, config.StorageSecret, config.StorageTableName);
			checkMeterProviders = new Dictionary<nMeterTypes, ICheckMeterProvider>();
			checkMeterProviders.Add(nMeterTypes.Heartbeat, new HeartbeatCheckMeterProvider());
		}

		public List<MeterCheckResult> Check()
		{
			// get all instances
			var meterInstances = repository.LoadInstances();

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
