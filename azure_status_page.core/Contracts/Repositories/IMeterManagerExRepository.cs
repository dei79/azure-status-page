using System;
using System.Collections.Generic;
using azure_status_page.client.Contracts.Repositories;
using azure_status_page.core.Models;

namespace azure_status_page.core.Repositories
{
	public interface IMeterManagerExRepository : IMeterInstanceRepository
	{
		void CreateOrUpdateServerSideDefinition(MeterDefinitionServerBased definition);

		List<MeterDefinitionServerBased> LoadServerBasedDefinitions();

		MeterDefinitionServerBased LoadServerBasedDefinition(string MeterId, string MeterCheckType);

		void DeleteSideDefinition(string MeterId, string MeterCheckType);
	}
}
