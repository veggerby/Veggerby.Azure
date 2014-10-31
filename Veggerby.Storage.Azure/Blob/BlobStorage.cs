using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Veggerby.Storage.Azure.Blob
{
    public class BlobStorage : IBlobStorage
    {
        private readonly CloudBlobContainer _BlobContainer;

        public BlobStorage(string blobContainer, string connectionString = null, StorageInitializeManager storageInitializeManager = null)
        {
            var storageAccount = !string.IsNullOrEmpty(connectionString)
                ? CloudStorageAccount.Parse(connectionString)
                : CloudStorageAccount.DevelopmentStorageAccount;
            var blobClient = storageAccount.CreateCloudBlobClient();
            _BlobContainer = blobClient.GetContainerReference(blobContainer);

            if (storageInitializeManager == null || storageInitializeManager.HasBeenInitialized(blobContainer))
            {
                return;
            }

            _BlobContainer.CreateIfNotExists();
            storageInitializeManager.SetAsInitialized(blobContainer);
        }

        public async Task InitializeAsync()
        {
            await BlobContainer.CreateIfNotExistsAsync();
        } 

        protected CloudBlobContainer BlobContainer
        {
            get { return _BlobContainer; }
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

            //stream.Position = 0;
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

        public async Task<Stream> GetAsync(string directory, string filename)
        {
            var block = GetBlockBlobReference(directory, filename);

            if (!(await block.ExistsAsync()))
            {
                return null;
            }

            var stream = new MemoryStream();

            await block.DownloadToStreamAsync(stream);

            stream.Position = 0;
            return stream;
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
