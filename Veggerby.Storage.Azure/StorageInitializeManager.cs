using System.Collections.Generic;

namespace Veggerby.Storage.Azure
{
    public class StorageInitializeManager
    {
        private readonly IList<string> _hasBeenInitialized;

        public StorageInitializeManager()
        {
            _hasBeenInitialized = new List<string>();
        }

        public bool HasBeenInitialized(string storageContainer)
        {
            return _hasBeenInitialized.Contains(storageContainer);
        }

        public void SetAsInitialized(string storageContainer)
        {
            _hasBeenInitialized.Add(storageContainer);
        }
    }
}
