using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Veggerby.Storage.Azure.Blob
{
    public class BlobStorage : IBlobStorage
    {
        private readonly CloudBlobContainer _blobContainer;

        public BlobStorage(string blobContainer, string connectionString = null, StorageInitializeManager storageInitializeManager = null)
        {
            var storageAccount = !string.IsNullOrEmpty(connectionString)
                ? CloudStorageAccount.Parse(connectionString)
                : CloudStorageAccount.DevelopmentStorageAccount;
            var blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference(blobContainer);

            if (storageInitializeManager == null || storageInitializeManager.HasBeenInitialized(blobContainer))
            {
                return;
            }

            _blobContainer.CreateIfNotExists();
            storageInitializeManager.SetAsInitialized(blobContainer);
        }

        protected CloudBlobContainer BlobContainer
        {
            get { return _blobContainer; }
        }

        private CloudBlockBlob GetBlockBlobReference(string directory, string filename)
        {
            if (string.IsNullOrEmpty(directory)) return BlobContainer.GetBlockBlobReference(filename);

            var dir = BlobContainer.GetDirectoryReference(directory);
            return dir.GetBlockBlobReference(filename);
        }

        public async Task<Uri> AddAsync(Stream stream, string directory, string filename, string contentType = "application/octet-stream", IDictionary<string, string> metadata = null)
        {
            var block = GetBlockBlobReference(directory, filename);

            block.Properties.ContentType = contentType;
            if (metadata != null)
            {
                foreach (var m in metadata)
                {
                    block.Metadata.Add(m);
                }
            }

            await block.UploadFromStreamAsync(stream);

            return block.Uri;
        }

        private async Task<Stream> GetAsync(CloudBlockBlob block)
        {
            if (!(await block.ExistsAsync()))
            {
                return null;
            }

            var stream = new MemoryStream();

            await block.DownloadToStreamAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task<Stream> GetAsync(Uri uri)
        {
            var block = new CloudBlockBlob(uri);
            return await GetAsync(block);
        }

        public async Task<Stream> GetAsync(string directory, string filename)
        {
            var block = GetBlockBlobReference(directory, filename);
            return await GetAsync(block);
        }

        public async Task AddMetaDataAsync(string directory, string filename, IDictionary<string, string> metadata)
        {
            var block = GetBlockBlobReference(directory, filename);

            if (!(await block.ExistsAsync()))
            {
                return;
            }

            await block.FetchAttributesAsync();

            foreach (var entry in metadata)
            {
                block.Metadata[entry.Key] = entry.Value;
            }

            await block.SetMetadataAsync();
        }

        public async Task<bool> ExistsAsync(string directory, string filename)
        {
            var block = GetBlockBlobReference(directory, filename);
            return await block.ExistsAsync();
        }

        public async Task<BlobInfo> GetInfoAsync(string directory, string filename)
        {
            var block = GetBlockBlobReference(directory, filename);

            if (!(await block.ExistsAsync()))
            {
                return null;
            }

            await block.FetchAttributesAsync();
            
            return new BlobInfo
            {
                BlobUri = block.Uri,
                Properties = block.Properties,
                Metadata = block.Metadata
            };
        }

        public async Task<IEnumerable<Uri>> ListAsync(string directory)
        {
            var blobDirectory = BlobContainer.GetDirectoryReference(directory);

            var token = new BlobContinuationToken();
            var segment = await blobDirectory.ListBlobsSegmentedAsync(token);

            if (segment == null || segment.Results == null || !segment.Results.Any())
            {
                return Enumerable.Empty<Uri>();
            }

            var result = segment.Results.Select(x => x.Uri).ToList();

            while (segment.ContinuationToken != null)
            {
                segment = await blobDirectory.ListBlobsSegmentedAsync(segment.ContinuationToken);
                result.AddRange(segment.Results.Select(x => x.Uri));
            }

            return result;
        }

        public async Task<bool> DeleteAsync(string directory, string filename)
        {
            var block = GetBlockBlobReference(directory, filename);

            if (!(await block.ExistsAsync()))
            {
                return false;
            }

            return await block.DeleteIfExistsAsync();
        }
    }
}
