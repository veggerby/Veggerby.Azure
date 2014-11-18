using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Storage.Azure.Table.Query
{
    public class StorageQuery<T> where T : ITableEntity, new()
    {
        protected TableQuery<T> Query;

        public StorageQuery()
        {
            Query = new TableQuery<T>();
        }

        public async virtual Task<IEnumerable<T>> ExecuteOnAsync(CloudTable table, int count = -1)
        {
            var token = new TableContinuationToken();
            var segment = await table.ExecuteQuerySegmentedAsync(Query, token);
            var result = new List<T>(segment);

            while (segment.ContinuationToken != null && (count < 0 || result.Count < count))
            {
                segment = await table.ExecuteQuerySegmentedAsync(Query, segment.ContinuationToken);
                result.AddRange(segment);
            }

            return count < 0 ? result : result.Take(count);
        }

        protected string InclusiveRangeFilter(string key, string from, string to)
        {
            var low = TableQuery.GenerateFilterCondition(key, QueryComparisons.GreaterThanOrEqual, from);
            var high = TableQuery.GenerateFilterCondition(key, QueryComparisons.LessThanOrEqual, to);
            return TableQuery.CombineFilters(low, TableOperators.And, high);
        }
    }
}