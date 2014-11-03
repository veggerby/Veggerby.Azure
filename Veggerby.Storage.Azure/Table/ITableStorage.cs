using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Veggerby.Storage.Azure.Table.Query;

namespace Veggerby.Storage.Azure.Table
{
    public interface ITableStorage<T> where T : ITableEntity, new()
    {
        Task<T> GetAsync(string partitionKey, string rowKey);
        Task<IEnumerable<T>> GetAsync(IEnumerable<TableEntityKey> keys);
        
        Task<string> InsertAsync(T entity);
        Task InsertAsync(IEnumerable<T> entities);
        
        Task<string> ReplaceAsync(T entity);
        
        Task<string> InsertOrReplaceAsync(T entity);
        Task InsertOrReplaceAsync(IEnumerable<T> entities);
        
        Task<string> DeleteAsync(T entity);
        Task DeleteAsync(IEnumerable<T> entities);
        
        Task<IEnumerable<T>> ListAsync(string partitionKey, int count = -1);
        Task<IEnumerable<T>> ListByRowKeyAsync(string rowKey, int count = -1);
        
        Task<IEnumerable<T>> ListAllAsync();
        Task<QuerySegment<T>> ListAllSegmentedAsync(string continuationToken, int pageCount, string queryFilter = null);

    }
}