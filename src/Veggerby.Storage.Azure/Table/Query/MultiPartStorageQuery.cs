using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Storage.Azure.Table.Query
{
    public abstract class MultiplePartStorageQuery<TPart, T> : StorageQuery<T> where T : ITableEntity, new()
    {
        public MultiplePartStorageQuery(IEnumerable<TPart> parts)
        {
            Query = Query.Where(GenerateFilterCondition(parts));
        }

        protected abstract string GetPart(TPart part);

        protected string GenerateFilterCondition(IEnumerable<TPart> parts)
        {
            var objlist = parts.ToList();

            var count = objlist.Count();

            if (count == 0)
            {
                return string.Empty;
            }

            if (count == 1)
            {
                var single = objlist.Single();
                return GetPart(single);
            }

            var middle = count / 2;

            var left = GenerateFilterCondition(objlist.Take(middle));
            var right = GenerateFilterCondition(objlist.Skip(middle));

            if (string.IsNullOrEmpty(left))
            {
                return right;
            }

            if (string.IsNullOrEmpty(right))
            {
                return left;
            }

            return left.Or(right);
        }
    }
}