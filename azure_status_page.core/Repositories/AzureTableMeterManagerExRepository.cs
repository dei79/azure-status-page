using System;
using System.Collections.Generic;
using azure_status_page.client.Models;
using azure_status_page.client.Repositories;
using azure_status_page.core.Models;

namespace azure_status_page.core.Repositories
{
	public class AzureTableMeterManagerExRepository : AzureTableMeterManagerRepository, IMeterManagerExRepository
	{
		public AzureTableMeterManagerExRepository(string storageKey, string storageSecret, string instanceTableName)
			: base(storageKey, storageSecret, instanceTableName)
		{
		}

		public void CreateOrUpdateServerSideDefinition(MeterDefinitionServerBased definition)
		{
			// get the entity
			var entity = AzureTableMeterDefinitionServerBased.FromModel(definition);

			// merge
			InsertOrMerge(entity, "MeterDefinitionServerBased");
		}

		public void DeleteSideDefinition(string MeterId, string MeterCheckType)
		{
			DeleteEntity("MeterDefinitionServerBased", MeterId, MeterCheckType);
		}

		public MeterDefinitionServerBased LoadServerBasedDefinition(string MeterId, string MeterCheckType)
		{
			var entity = LoadInstance<AzureTableMeterDefinitionServerBased>("MeterDefinitionServerBased", MeterId, MeterCheckType);
			return entity.ToModel();
		}

		public List<MeterDefinitionServerBased> LoadServerBasedDefinitions()
		{
			// query
			var queryResult = LoadInstances<AzureTableMeterDefinitionServerBased>("MeterDefinitionServerBased");

			// Print the fields for each customer.
			var result = new List<MeterDefinitionServerBased>();
			foreach (AzureTableMeterDefinitionServerBased entity in queryResult)
			{
				result.Add(entity.ToModel());
			}

			return result;
		}
	}
}
