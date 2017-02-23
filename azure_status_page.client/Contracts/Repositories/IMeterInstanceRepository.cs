using System;
using System.Collections.Generic;
using azure_status_page.client.Models;

namespace azure_status_page.client.Contracts.Repositories
{
	public interface IMeterInstanceRepository
	{
		List<MeterInstance> LoadInstances();

		void StoreInstance(MeterInstance instance);
	}
}
