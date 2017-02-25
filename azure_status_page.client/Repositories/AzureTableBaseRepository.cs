using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

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

		protected void InsertOrMerge(TableEntity entity, string tableName)
		{
			// Create the table client.
			CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

			// Retrieve a reference to the table.
			CloudTable table = tableClient.GetTableReference(tableName);

			// Create the batch operation.
			TableBatchOperation batchOperation = new TableBatchOperation();

			// Add the entity to the batch
			batchOperation.InsertOrMerge(entity);

			// Merge
			try
			{
				table.ExecuteBatch(batchOperation);
			}
			catch (StorageException e)
			{
				if (e.RequestInformation.HttpStatusCode == 404)
				{					
					// Create the table if it doesn't exist.
					table.CreateIfNotExists();

					// do the insert again 
					table.ExecuteBatch(batchOperation);
				}
			}

		}

		public IEnumerable<T> LoadInstances<T>(string tableName) where T : TableEntity, new()
		{
			// Create the table client.
			CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

			// Retrieve a reference to the table.
			CloudTable table = tableClient.GetTableReference(tableName);

			// Construct the query operation for all customer entities where PartitionKey="Smith".
			var query = new TableQuery<T>();

			// query
			return table.ExecuteQuery(query);
		}

		public T LoadInstance<T>(string tableName, string partitionKey, string rowKey) where T : TableEntity, new()
		{
			// Create the table client.
			CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

			// Retrieve a reference to the table.
			CloudTable table = tableClient.GetTableReference(tableName);

			// Create a retrieve operation that takes a customer entity.
			TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

			// Execute the retrieve operation.
			TableResult retrievedResult = table.Execute(retrieveOperation);

			// Print the phone number of the result.
			return retrievedResult.Result as T;
		}

		public void DeleteEntity(string tableName, string partitonKey, string rowKey)
		{
			// Create the table client.
			CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

			// Retrieve a reference to the table.
			CloudTable table = tableClient.GetTableReference(tableName);

			// Create a retrieve operation that expects a customer entity.
			TableOperation retrieveOperation = TableOperation.Retrieve<TableEntity>(partitonKey, rowKey);

			// Execute the operation.
			TableResult retrievedResult = table.Execute(retrieveOperation);

			// Assign the result to a CustomerEntity.
			var deleteEntity = (TableEntity)retrievedResult.Result;

			// Create the Delete TableOperation.
			if (deleteEntity != null)
			{
				TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

				// Execute the operation.
				table.Execute(deleteOperation);
			}
		}
	}
}
