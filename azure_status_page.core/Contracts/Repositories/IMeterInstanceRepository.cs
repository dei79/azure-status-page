using System;
using System.Collections.Generic;

namespace azure_status_page.core
{
	public interface IMeterInstanceRepository
	{
		List<MeterInstance> LoadInstances();	
	}
}
