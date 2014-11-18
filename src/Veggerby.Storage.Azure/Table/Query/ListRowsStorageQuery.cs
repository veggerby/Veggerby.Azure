using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Storage.Azure.Table.Query
{
    public class ListRowsStorageQuery<T> : StorageQuery<T> where T : ITableEntity, new()
    {
        public ListRowsStorageQuery(string rowKey)
        {
            Query = Query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));
        }
    }
}   