using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Veggerby.Storage.Azure.Table.Query
{
    public class QuerySegment<T> where T : ITableEntity
    {
        public string ContinuationToken { get; private set; }
        public IEnumerable<T> Items { get; private set; }

        public QuerySegment(TableQuerySegment<T> tableQuerySegment)
        {
            ContinuationToken = tableQuerySegment.ContinuationToken != null 
                ? JsonConvert.SerializeObject(tableQuerySegment.ContinuationToken) 
                : null;

            Items = tableQuerySegment.Results;
        }
    }
}