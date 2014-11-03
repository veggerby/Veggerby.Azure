using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Veggerby.Storage.Azure.Table.Query;

namespace Veggerby.Storage.Azure.Table
{
    public class TableStorage<T> : ITableStorage<T> where T : ITableEntity, new()
    {
        private readonly CloudTable _Table;
        private readonly CloudTableClient _cloudTableClient;

        public CloudTableClient CloudTableClient { get { return _cloudTableClient; } }

        public TableStorage(string tableName = null, string connectionString = null, StorageInitializeManager storageInitializeManager = null)
        {
            var storageAccount = !string.IsNullOrEmpty(connectionString)
                ? CloudStorageAccount.Parse(connectionString)
                : CloudStorageAccount.DevelopmentStorageAccount;
            _cloudTableClient = storageAccount.CreateCloudTableClient();

            tableName = tableName ?? typeof(T).GetTableName();

            _Table = _cloudTableClient.GetTableReference(tableName);

            if (storageInitializeManager == null || storageInitializeManager.HasBeenInitialized(tableName))
            {
                return;
            }

            _Table.CreateIfNotExists();
            storageInitializeManager.SetAsInitialized(tableName);
        }

        protected CloudTable Table
        {
            get { return _Table; }
        }

        protected async Task<string> ExecuteAsync(TableOperation operation)
        {
            var result = await Table.ExecuteAsync(operation);
            return result.Etag;
        }

        protected async Task<T> ExecuteItemAsync(TableOperation operation)
        {
            var result = await Table.ExecuteAsync(operation);
            return result.Result is T ? (T)result.Result : default(T);
        }

        public async Task ExecuteBatchAsync(IEnumerable<T> entities, Func<T, TableOperation> operation,
            int batchSize = 100)
        {
            var list = entities as IList<T> ?? entities.ToList();
            while (list.Any())
            {
                var batch = new TableBatchOperation();
                foreach (var item in list.Take(batchSize))
                {
                    batch.Add(operation(item));
                }

                list = list.Skip(batchSize).ToList();

                await Table.ExecuteBatchAsync(batch);
            }
        }

        public async virtual Task<T> GetAsync(string partitionKey, string rowKey)
        {
            return await ExecuteItemAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
        }

        public async virtual Task<string> InsertAsync(T entity)
        {
            return await ExecuteAsync(TableOperation.Insert(entity));
        }

        public async virtual Task InsertAsync(IEnumerable<T> entities)
        {
            await ExecuteBatchAsync(entities, x => TableOperation.Insert(x));
        }

        public async virtual Task<string> ReplaceAsync(T entity)
        {
            return await ExecuteAsync(TableOperation.Replace(entity));
        }

        public async virtual Task<string> InsertOrReplaceAsync(T entity)
        {
            return await ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public async virtual Task InsertOrReplaceAsync(IEnumerable<T> entities)
        {
            await ExecuteBatchAsync(entities, x => TableOperation.InsertOrReplace(x));
        }

        public async virtual Task<string> DeleteAsync(T entity)
        {
            return await ExecuteAsync(TableOperation.Delete(entity));
        }

        public async virtual Task DeleteAsync(IEnumerable<T> entities)
        {
            await ExecuteBatchAsync(entities, x => TableOperation.Delete(x));
        }

        public async virtual Task<IEnumerable<T>> ListAsync(string partitionKey, int count = -1)
        {
            return await new ListPartitionStorageQuery<T>(partitionKey).ExecuteOnAsync(Table, count);
        }

        public async virtual Task<IEnumerable<T>> ListByRowKeyAsync(string rowKey, int count = -1)
        {
            return await new ListRowsStorageQuery<T>(rowKey).ExecuteOnAsync(Table, count);
        }

        public async virtual Task<IEnumerable<T>> GetAsync(IEnumerable<TableEntityKey> keys)
        {
            if (keys == null || !keys.Any())
            {
                return Enumerable.Empty<T>();
            }

            return await new MultipleEntityStorageQuery<T>(keys).ExecuteOnAsync(Table);
        }

        public Task<IEnumerable<T>> ListAllAsync()
        {
            return new StorageQuery<T>().ExecuteOnAsync(Table);
        }

        public async Task<QuerySegment<T>> ListAllSegmentedAsync(string continuationToken, int pageCount, string queryFilter = null)
        {
            var token = string.IsNullOrWhiteSpace(continuationToken)
            ? new TableContinuationToken()
            : JsonConvert.DeserializeObject<TableContinuationToken>(continuationToken);

            var tableQuery = new TableQuery<T> { TakeCount = pageCount, FilterString = queryFilter };

            var queryResults = await Table.ExecuteQuerySegmentedAsync(tableQuery, token).ConfigureAwait(false);

            return new QuerySegment<T>(queryResults);
        }
    }
}
