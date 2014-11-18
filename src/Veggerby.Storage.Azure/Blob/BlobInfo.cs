using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Veggerby.Storage.Azure.Blob
{
    public class BlobInfo
    {
        public Uri BlobUri { get; set; }
        public BlobProperties Properties { get; set; }
        public IDictionary<string, string> Metadata { get; set; }

        public string GetMetaData(string key, string @default = null)
        {
            return Metadata != null && Metadata.ContainsKey(key) ? Metadata[key] : @default;
        }
    }
}