using System;
using System.IO;
using azure_status_page.client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace azure_status_page.client.demo
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// define the meter ids
			var meterHeartBeatMeterId = "3ac0ab14-561f-452b-aee7-85ef79d6eef1";

			// Load the storage credentials from external file
			// { "key": "xxx", secret="yyy" }
			var credentials = new { key = "", secret = "" };
			dynamic d = JsonConvert.DeserializeObject(File.ReadAllText(Path.Combine("..", "..", "..", "credentials.json")), credentials.GetType());
			                          
			// Initial configuration of the MeterManager
			MeterManager.Instance.ConfigureAzureTableStoreRepository(d.key, d.secret, "MeterInformation");

			// Define a meter, e.g. a heartbeat for a continous job
			var meterId = MeterManager.Instance.RegisterMeter(meterHeartBeatMeterId, "Image Processing", "Background Processing", nMeterTypes.Heartbeat, 500).MeterId;

			// Update this meter with the hearbeat
			MeterManager.Instance.UpdateMeter(meterId, "Node01.WebJob01.ImgProcessing");
		}
	}
}
