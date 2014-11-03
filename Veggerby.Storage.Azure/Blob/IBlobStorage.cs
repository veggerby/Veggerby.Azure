using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Veggerby.Storage.Azure.Blob
{
    public interface IBlobStorage
    {
        Task<Uri> AddAsync(Stream stream, string directory, string filename, string contentType = "application/octet-stream", IDictionary<string, string> metadata = null);
        Task<Stream> GetAsync(Uri uri);
        Task<Stream> GetAsync(string directory, string filename);
        Task AddMetaDataAsync(string directory, string filename, IDictionary<string, string> metadata);
        Task<bool> ExistsAsync(string directory, string filename);
        Task<BlobInfo> GetInfoAsync(string directory, string filename);
        Task<bool> DeleteAsync(string directory, string filename);
        Task<IEnumerable<Uri>> ListAsync(string directory);
    }
}