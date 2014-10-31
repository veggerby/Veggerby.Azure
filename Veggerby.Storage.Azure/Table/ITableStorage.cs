using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Veggerby.Storage.Azure.Table.Query;

namespace Veggerby.Storage.Azure.Table
{
    public interface ITableStorage<T> where T : ITableEntity, new()
    {
        Task InitializeAsync();

        T Get(string partitionKey, string rowKey);
        Task<T> GetAsync(string partitionKey, string rowKey);
        IEnumerable<T> Get(IEnumerable<TableEntityKey> keys);
        Task<IEnumerable<T>> GetAsync(IEnumerable<TableEntityKey> keys);
        
        string Insert(T entity);
        void Insert(IEnumerable<T> entities);
        Task<string> InsertAsync(T entity);
        Task InsertAsync(IEnumerable<T> entities);
        
        string Replace(T entity);
        Task<string> ReplaceAsync(T entity);
        
        string InsertOrReplace(T entity);
        void InsertOrReplace(IEnumerable<T> entities);
        Task<string> InsertOrReplaceAsync(T entity);
        Task InsertOrReplaceAsync(IEnumerable<T> entities);
        
        string Delete(T entity);
        Task<string> DeleteAsync(T entity);
        void Delete(IEnumerable<T> entities);
        Task DeleteAsync(IEnumerable<T> entities);
        
        IEnumerable<T> List(string partitionKey);
        Task<IEnumerable<T>> ListAsync(string partitionKey, int count = -1);
        IEnumerable<T> ListByRowKey(string rowKey);
        Task<IEnumerable<T>> ListByRowKeyAsync(string rowKey, int count = -1);
        
        Task<IEnumerable<T>> ListAllAsync();
        Task<QuerySegment<T>> ListAllSegmentedAsync(string continuationToken, int pageCount, string queryFilter = null);

    }
}