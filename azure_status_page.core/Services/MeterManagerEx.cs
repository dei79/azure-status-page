using System;
using System.Collections.Generic;
using System.Net;
using azure_status_page.client.Contracts.Repositories;
using azure_status_page.client.Models;
using azure_status_page.core.Models;
using azure_status_page.core.Repositories;

namespace azure_status_page.core
{	
	public class MeterManagerEx 
	{
		private IMeterManagerExRepository MeterManagerRepository { get; set; }

		public MeterManagerEx(IMeterManagerExRepository meterManagerRepository)
		{
			MeterManagerRepository = meterManagerRepository;
		}

		public MeterManagerEx(MeterStorageConfigurationModel config)
		{
			MeterManagerRepository = new AzureTableMeterManagerExRepository(config.StorageKey, config.StorageSecret, config.StorageTableName);
		}

		public List<MeterDefinition> GetClientSideDefinitions()
		{
			// Load MeterInstances
			var meterInstances = MeterManagerRepository.LoadInstances();

			// Aggregate all instances to the correct meterdefinitions
			var meterHash = new Dictionary<string, MeterDefinition>();
			var result = new List<MeterDefinition>();
			foreach (var meterInstacen in meterInstances)
			{
				if (meterInstacen.MeterInstanceId.StartsWith("Server.", StringComparison.CurrentCulture))
					continue;
				
				if (!meterHash.ContainsKey(meterInstacen.MeterId))
				{
					meterHash.Add(meterInstacen.MeterId, meterInstacen);
					result.Add(new MeterDefinition(meterInstacen));
				}
			}

			// order 
			result.Sort((x, y) => { return x.MeterDisplayOrder.CompareTo(y.MeterDisplayOrder); });

			// done
			return result;
		}

		public List<MeterDefinitionServerBased> GetServerBasedDefinitions()
		{
			return MeterManagerRepository.LoadServerBasedDefinitions();
		}

		public MeterDefinitionServerBased GetServerBasedDefinition(string MeterId, string MeterCheckType)
		{
			return MeterManagerRepository.LoadServerBasedDefinition(MeterId, MeterCheckType);
		}

		public void CreateOrUpdateServerSideDefinition(MeterDefinitionServerBased definition)
		{
			MeterManagerRepository.CreateOrUpdateServerSideDefinition(definition);
		}

		public void DeleteServerSideDefinition(string MeterId, string MeterCheckType)
		{
			// delete the instance
			MeterManagerRepository.DeleteInstance(MeterId, "Server." + MeterId + ".WebCheck");
			MeterManagerRepository.DeleteInstance(MeterId, "Server." + MeterId + ".HeartBeat");

			// delete the server side definition
			MeterManagerRepository.DeleteSideDefinition(MeterId, MeterCheckType);
		}

		public void ExecuteServerSideChecks()
		{
			// load the meter definitions
			var definitions = MeterManagerRepository.LoadServerBasedDefinitions();

			// execute the check
			foreach (var definition in definitions)
			{
				// generate the instance for the heartbeat
				var meterInstance = new MeterInstance(definition);
				meterInstance.MeterValue = definition.MeterServerCheckInterval;
				meterInstance.MeterInstanceId = "Server." + definition.MeterId + ".HeartBeat";
				meterInstance.MeterInstanceTimestamp = DateTime.Now;
				MeterManagerRepository.StoreInstance(meterInstance);

				// generate an instance for the http return value
				var meterInstanceHttpResult = new MeterInstance(definition);
				meterInstanceHttpResult.MeterType = nMeterTypes.EqualsValue;
				meterInstanceHttpResult.MeterValue = definition.MeterValue;
				meterInstanceHttpResult.MeterInstanceId = "Server." + definition.MeterId + ".WebCheck";
				meterInstanceHttpResult.MeterInstanceTimestamp = DateTime.Now;

				// check 
				try
				{
					var request = WebRequest.CreateHttp(definition.MeterServerCheckInformation);
					request.Method = "HEAD";
					request.Timeout = 1000;
					var response = request.GetResponse() as HttpWebResponse;
					meterInstanceHttpResult.MeterInstanceValue = Convert.ToInt32(response.StatusCode);
				}
				catch (Exception e)
				{
					if (e is WebException && (((WebException)e).Response as HttpWebResponse) != null)
					{
						meterInstanceHttpResult.MeterInstanceValue = Convert.ToInt32(((e as WebException).Response as HttpWebResponse).StatusCode);
					}
					else
					{
						meterInstanceHttpResult.MeterInstanceValue = -1;
					}
				}

				// Logging 
				Console.WriteLine("Checkking URL: " + definition.MeterServerCheckInformation + " - Result: " + meterInstanceHttpResult.MeterInstanceValue);

				// generate instances per check 
				MeterManagerRepository.StoreInstance(meterInstanceHttpResult);
			}

		}
	}
}
