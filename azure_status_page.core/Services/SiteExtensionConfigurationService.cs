using System;
using System.IO;
using Newtonsoft.Json;

namespace azure_status_page.core
{
	public class SiteExtensionConfigurationService
	{		
		private string GetSettingsFileName()
		{
			return Path.Combine(AzureEnvironmentService.DataDirectory, "settings.json");
		}

		public Boolean VerifyAndStoreConfiguration(MeterStorageConfigurationModel configuration)
		{
			// Verify the config 
			configuration.Valid = true;

			// build the file name 
			var settingsFileName = GetSettingsFileName();

			// serialize JSON directly to a file
			using(StreamWriter file = File.CreateText(settingsFileName))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, configuration);
			}

			// done
			return true;
		}

		public MeterStorageConfigurationModel LoadConfiguration()
		{
			// build the file name 
			var settingsFileName = GetSettingsFileName();

			// check if file exists
			if (File.Exists(settingsFileName)) 
			{
				// deserialize JSON
				using (StreamReader file = File.OpenText(settingsFileName))
				{
					JsonSerializer serializer = new JsonSerializer();
					return (MeterStorageConfigurationModel)serializer.Deserialize(file, typeof(MeterStorageConfigurationModel));
				}
			}
			else
			{
				return new MeterStorageConfigurationModel() { StorageTableName = "MeterInformation", Valid = false };
			}
		}
	}
}
