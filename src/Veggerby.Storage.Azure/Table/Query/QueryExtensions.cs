using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Storage.Azure.Table.Query
{
    public static class QueryExtensions
    {
        public static string Query(this string field, string operation, string value)
        {
            return TableQuery.GenerateFilterCondition(field, operation, value);
        }

        public static string Query(this string field, string operation, DateTime value)
        {
            return TableQuery.GenerateFilterConditionForDate(field, operation, value);
        }

        public static string Query(this string field, string operation, int value)
        {
            return TableQuery.GenerateFilterConditionForInt(field, operation, value);
        }

        public static string Query(this string field, string operation, double value)
        {
            return TableQuery.GenerateFilterConditionForDouble(field, operation, value);
        }

        public static string Query(this string field, string operation, long value)
        {
            return TableQuery.GenerateFilterConditionForLong(field, operation, value);
        }

        public static string Query(this string field, string operation, bool value)
        {
            return TableQuery.GenerateFilterConditionForBool(field, operation, value);
        }

        public static string Query(this string field, string operation, Guid value)
        {
            return TableQuery.GenerateFilterConditionForGuid(field, operation, value);
        }

        public static string Equal(this string field, string value)
        {
            return field.Query(QueryComparisons.Equal, value);
        }

        public static string Equal(this string field, DateTime value)
        {
            return field.Query(QueryComparisons.Equal, value);
        }

        public static string Equal(this string field, int value)
        {
            return field.Query(QueryComparisons.Equal, value);
        }

        public static string Equal(this string field, double value)
        {
            return field.Query(QueryComparisons.Equal, value);
        }

        public static string Equal(this string field, long value)
        {
            return field.Query(QueryComparisons.Equal, value);
        }

        public static string Equal(this string field, bool value)
        {
            return field.Query(QueryComparisons.Equal, value);
        }

        public static string Equal(this string field, Guid value)
        {
            return field.Query(QueryComparisons.Equal, value);
        }

        public static string LessThan(this string field, string value)
        {
            return field.Query(QueryComparisons.LessThan, value);
        }

        public static string LessThan(this string field, DateTime value)
        {
            return field.Query(QueryComparisons.LessThan, value);
        }

        public static string LessThan(this string field, int value)
        {
            return field.Query(QueryComparisons.LessThan, value);
        }

        public static string LessThan(this string field, double value)
        {
            return field.Query(QueryComparisons.LessThan, value);
        }

        public static string LessThan(this string field, long value)
        {
            return field.Query(QueryComparisons.LessThan, value);
        }

        public static string LessThan(this string field, bool value)
        {
            return field.Query(QueryComparisons.LessThan, value);
        }

        public static string LessThan(this string field, Guid value)
        {
            return field.Query(QueryComparisons.LessThan, value);
        }

        public static string LessThanOrEqual(this string field, string value)
        {
            return field.Query(QueryComparisons.LessThanOrEqual, value);
        }

        public static string LessThanOrEqual(this string field, DateTime value)
        {
            return field.Query(QueryComparisons.LessThanOrEqual, value);
        }

        public static string LessThanOrEqual(this string field, int value)
        {
            return field.Query(QueryComparisons.LessThanOrEqual, value);
        }

        public static string LessThanOrEqual(this string field, double value)
        {
            return field.Query(QueryComparisons.LessThanOrEqual, value);
        }

        public static string LessThanOrEqual(this string field, long value)
        {
            return field.Query(QueryComparisons.LessThanOrEqual, value);
        }

        public static string LessThanOrEqual(this string field, bool value)
        {
            return field.Query(QueryComparisons.LessThanOrEqual, value);
        }

        public static string LessThanOrEqual(this string field, Guid value)
        {
            return field.Query(QueryComparisons.LessThanOrEqual, value);
        }

        public static string GreaterThan(this string field, string value)
        {
            return field.Query(QueryComparisons.GreaterThan, value);
        }

        public static string GreaterThan(this string field, DateTime value)
        {
            return field.Query(QueryComparisons.GreaterThan, value);
        }

        public static string GreaterThan(this string field, int value)
        {
            return field.Query(QueryComparisons.GreaterThan, value);
        }

        public static string GreaterThan(this string field, double value)
        {
            return field.Query(QueryComparisons.GreaterThan, value);
        }

        public static string GreaterThan(this string field, long value)
        {
            return field.Query(QueryComparisons.GreaterThan, value);
        }

        public static string GreaterThan(this string field, bool value)
        {
            return field.Query(QueryComparisons.GreaterThan, value);
        }

        public static string GreaterThan(this string field, Guid value)
        {
            return field.Query(QueryComparisons.GreaterThan, value);
        }

        public static string GreaterThanOrEqual(this string field, string value)
        {
            return field.Query(QueryComparisons.GreaterThanOrEqual, value);
        }

        public static string GreaterThanOrEqual(this string field, DateTime value)
        {
            return field.Query(QueryComparisons.GreaterThanOrEqual, value);
        }

        public static string GreaterThanOrEqual(this string field, int value)
        {
            return field.Query(QueryComparisons.GreaterThanOrEqual, value);
        }

        public static string GreaterThanOrEqual(this string field, double value)
        {
            return field.Query(QueryComparisons.GreaterThanOrEqual, value);
        }

        public static string GreaterThanOrEqual(this string field, long value)
        {
            return field.Query(QueryComparisons.GreaterThanOrEqual, value);
        }

        public static string GreaterThanOrEqual(this string field, bool value)
        {
            return field.Query(QueryComparisons.GreaterThanOrEqual, value);
        }

        public static string GreaterThanOrEqual(this string field, Guid value)
        {
            return field.Query(QueryComparisons.GreaterThanOrEqual, value);
        }

        public static string And(this string part1, string part2)
        {
            return TableQuery.CombineFilters(
                part1, TableOperators.And, part2);
        }

        public static string Or(this string part1, string part2)
        {
            return TableQuery.CombineFilters(
                part1, TableOperators.Or, part2);
        }
    }
}
