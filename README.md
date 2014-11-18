Veggerby.Azure
==============

Azure Storage abstractions, based on [K. Scott Allen](http://odetocode.com/blogs/scott/archive/2014/02/27/some-basic-azure-table-storage-abstractions.aspx) ideas and extended to support Tables, Queues and Blobs. 

All methods are async only!

# Table Storage

## Entity

The Table Storage entity is declared as a normal [TableEntity](http://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.table.tableentity.aspx), e.g.

    public class FooEntity : TableEntity 
    {
    	public string Bar { get; set; }
    	public int Baz { get; set; }
    }

## Table Storage

Define your Table Storage:

	public class FooEntityStorage : TableStorage<FooEntity>
	{
		public FooEntityStorage(string tableName = null, string connectionString = null, StorageInitializeManager storageInitializeManager = null)
            : base(tableName, connectionString, storageInitializeManager)
        {
        }
	}

## Interfaces

Interface'ify the storage for Dependency Injection:

```C#
public interface IFooEntityStorage : ITableStorage<FooEntity>
{
}

public class FooEntityStorage : TableStorage<FooEntity>, IFooEntotyStorage
{
	...
}
```

## Querying

*Remember!* 
Querying in Azure Table Storage where not including PartitionKey (and possibly RowKey) has a huge performance impact. For optimal performance queries should include in descending order of performance impact (see [MSDN](http://msdn.microsoft.com/en-us/library/azure/hh508997.aspx#ytrus):

1. PartitionKey (exact) + RowKey (exact)
2. PartitionKey (exact) + RowKey (partial) + properties
3. PartitionKey (partial) + RowKey (partial) + properties
4. Everything else

Querying is done by subclassing StorageQuery<T>

```C#
public class ListFooEntityByBarStorageQuery: StorageQuery<FooEntity>
{
	public ListFooEntityByBarStorageQuery(string bar)
	{
		Query = Query.Where(
			TableQuery.GenerateFilterCondition("Bar", QueryComparisons.Equal, bar));
	}
}
```

Extend the FooEntityStorage class with the List (async) method.

```C#
public class FooEntityStorage : TableStorage<FooEntity>
{
	...

	public async Task<IEnumerable<FooEntity>> ListAsync(string bar)
	{
		return await new ListFooEntityByBarStorageQuery(bar).ExecuteOnAsync(Table);
	}
}
```

### Standard Queries

The following queries are implemented by default:

* [ListPartitionStorageQuery](src/Veggerby.Storage.Azure/Table/Query/ListPartitionStorageQuery.cs) - list entities in a single partition (see TableStorage.ListAsync())
* [ListRowsStorageQuery](src/Veggerby.Storage.Azure/Table/Query/ListRowsStorageQuery.cs) - list entities by row key (*significant performance impact*, see TableStorage.ListByRowKeyAsync())
* [MultipleEntityStorageQuery](src/Veggerby.Storage.Azure/Table/Query/MultipleEntityStorageQuery.cs) - get multiple entities by PartitionKey and RowKey (using [TableEntityKey](src/Veggerby.Storage.Azure/Table/TableEntityKey.cs) class, see TableStorage.GetAsync() overload)
* [MultiplePartitionStorageQuery](src/Veggerby.Storage.Azure/Table/Query/MultiplePartitionStorageQuery.cs) - list entities in multiple partitions (*performance impact*)
* [MultipleRowKeysStorageQuery](src/Veggerby.Storage.Azure/Table/Query/MultipleRowKeysStorageQuery.cs) - list entities with multiple RowKeys (*significant performance impact*)
* [DateRangeStorageQuery](src/Veggerby.Storage.Azure/Table/Query/DateRangeStorageQuery.cs) - list entities with having a specific property within a date/time range

By default the MultiplePartitionStorageQuery, MultipleRowKeysStorageQuery and DateRangeStorageQuery queries are not exposed on the TableStorage class because of the significant performance impact (although this also applies to ListRowsStorageQuery).