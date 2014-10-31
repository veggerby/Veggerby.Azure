namespace Veggerby.Storage.Azure.Table
{
    public class TableEntityKey
    {
        public TableEntityKey()
        {
        }

        public TableEntityKey(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
