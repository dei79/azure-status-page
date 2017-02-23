using System;
using System.Collections.Generic;
using azure_status_page.client.Contracts.Repositories;
using azure_status_page.client.Models;
using azure_status_page.client.Repositories;

namespace azure_status_page.client
{	
	public class MeterManager
	{			
		/// <summary>
		/// Singleton implementation of the MeterManager
		/// </summary>
		private static MeterManager instance;

		private MeterManager() { }

		public static MeterManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MeterManager();
				}

				return instance;
			}
		}

		/// <summary>
		/// Support for our standard repositories
		/// </summary>
		/// <value>The meter manager repository.</value>
		private IMeterManagerRepository meterManagerRepository { get; set; }

		public void ConfigureAzureTableStoreRepository(string storageKey, string storageSecret, string storageTable)
		{
			meterManagerRepository = new AzureTableMeterManagerRepository(storageKey, storageSecret, storageTable);
		}

		public void ConfigureRepository(IMeterManagerRepository repository)
		{
			meterManagerRepository = repository;
		}

		// properties
		private Dictionary<string, MeterDefinition> meterCache { get; set; } = new Dictionary<string, MeterDefinition>();

		/// <summary>
		/// Allows to register a new meter
		/// </summary>
		/// <param name="meterId">Meter identifier.</param>
		/// <param name="meterName">Meter name.</param>
		/// <param name="meterCategory">Meter category.</param>
		/// <param name="meterType">Meter type.</param>
		/// <param name="meterValue">Meter value.</param>
		public MeterDefinition RegisterMeter(String meterId, String meterName, String meterCategory, nMeterTypes meterType, Decimal meterValue)
		{
			// double check 
			if (meterCache.ContainsKey(meterId))
			{
				throw new Exception("Invalid MeterId, can't add the meter because the same meterid was used before for another meter");
			}

			// add the meter
			var meterDefinition = new MeterDefinition() { 
				MeterId = meterId, 
				MeterName = meterName, 
				MeterCategory = meterCategory, 
				MeterType = meterType, 
				MeterValue = meterValue 
			};

			meterCache.Add(meterId, meterDefinition);

			// done
			return meterDefinition;
		}

		/// <summary>
		/// Updates or creates an existing MeterInstance
		/// </summary>
		/// <param name="meterId">Meter identifier.</param>
		/// <param name="meterInstanceId">Meter instance identifier.</param>
		public MeterInstance UpdateMeter(String meterId, String meterInstanceId)
		{
			return this.UpdateMeter(meterId, meterInstanceId, Decimal.Zero);
		}

		public MeterInstance UpdateMeter(String meterId, String meterInstanceId, Decimal meterInstanceValue)
		{
			// lookup if we have the registered meter 
			if (!meterCache.ContainsKey(meterId))
			{
				throw new Exception("Meterdefinition for meter with id " + meterId + " is not registered");
			}

			// update the meter by creating a new meter instance and storing this in the repository
			var meterInstance = new MeterInstance(meterCache[meterId]);
			meterInstance.MeterInstanceId = meterInstanceId;
			meterInstance.MeterInstanceValue = meterInstanceValue;
			meterInstance.MeterInstanceTimestamp = DateTime.Now;

			// store the meter instance
			meterManagerRepository.StoreInstance(meterInstance);

			// done
			return meterInstance;
		}
	}
}
