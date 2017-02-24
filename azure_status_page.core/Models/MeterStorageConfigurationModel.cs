using System;
namespace azure_status_page.core
{
	public class MeterStorageConfigurationModel
	{
		// Storage Config
		public string StorageKey { get; set; }
		public string StorageSecret { get; set; }
		public string StorageTableName { get; set; }

		// Title, Brand, ....
		public string Title { get; set; }
		public string Brand { get; set; }
		public string FavIcon { get; set; }

		// PushOver Integration 
		public Boolean PushOverEnabled { get; set; }
		public string PushOverToken { get; set; }
		public string PushOverUser { get; set; }

		// Check if config is valid
		public Boolean Valid { get; set; }
	}
}
