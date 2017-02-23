using System;
using Microsoft.WindowsAzure.Storage;

namespace azure_status_page.client.Repositories
{
	public class AzureTableBaseRepository
	{
		protected string StorageTable { get; private set; }
		protected CloudStorageAccount StorageAccount { get; private set; }

		public AzureTableBaseRepository(string storageKey, string storageSecret, string storageTable)
		{
			StorageTable = storageTable;
			StorageAccount = CloudStorageAccount.Parse(String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageKey, storageSecret));
		}
	}
}
