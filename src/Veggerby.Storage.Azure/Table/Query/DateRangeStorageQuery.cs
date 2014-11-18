using System;
using Microsoft.WindowsAzure.Storage.Table;
using Queryable = System.Linq.Queryable;

namespace Veggerby.Storage.Azure.Table.Query
{
    public class DateRangeStorageQuery<T> : StorageQuery<T> where T: ITableEntity, new()
    {
        public DateRangeStorageQuery(string fieldId, DateTime? minValue, DateTime? maxValue)
        {
            if (minValue == null && maxValue == null)
            {
                throw new ArgumentException("minValue and maxValue cannot both be null", "minValue");
            }

            if (maxValue == null)
            {
                Query = Query.Where(
                    fieldId.GreaterThanOrEqual(minValue.Value));

            }
            else if (minValue == null)
            {
                Query = Query.Where(
                    fieldId.LessThanOrEqual(maxValue.Value));
            }
            else
            {
                Query = Query.Where(
                    fieldId.GreaterThanOrEqual(minValue.Value).Or(fieldId.LessThanOrEqual(maxValue.Value)));
            }
        }
    }
}
