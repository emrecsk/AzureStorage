using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace AzureStrorageLibrary.Services
{
    public class BlobStorage : IBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;

        public string BlobURL => "http://127.0.0.1:10000/devstoreaccount1";

        public BlobStorage()
        {
            _blobServiceClient = new BlobServiceClient(ConnectionString.Con);
        }
        public async Task<bool> UploadBlobAsync(EContainerName containerName, string blobName, Stream stream)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName.ToString().ToLower());
            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob); // make it public
            var blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.UploadAsync(stream);
            return response != null;
        }
        public async Task<bool> DeleteBlobAsync(EContainerName containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName.ToString().ToLower());
            var blobClient = containerClient.GetBlobClient(blobName);
            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<Stream> DownloadBlobAsync(EContainerName containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName.ToString().ToLower());
            var blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public Task<IEnumerable<string>> ListBlobsAsync(EContainerName containerName)
        {
            throw new NotImplementedException();
        }

        public async Task SetLog(string message, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(EContainerName.logs.ToString().ToLower());
            var appendBlobClient = containerClient.GetAppendBlobClient(blobName);
            await appendBlobClient.CreateIfNotExistsAsync();
            using (MemoryStream ms = new())
            {
                using (StreamWriter sw = new(ms))
                {
                    sw.WriteLine($"{DateTime.Now} - {message}");
                    sw.Flush();
                    ms.Position = 0;
                    await appendBlobClient.AppendBlockAsync(ms);
                }
            }
        }

        public async Task<List<string>> GetLogAsync(string blobName)
        {
            List<string> logs = new();
            var containerClient = _blobServiceClient.GetBlobContainerClient(EContainerName.logs.ToString().ToLower());
            await containerClient.CreateIfNotExistsAsync();
            var appendBlobClient = containerClient.GetAppendBlobClient(blobName);
            await appendBlobClient.CreateIfNotExistsAsync();

            var response = await appendBlobClient.DownloadAsync();
            if (response?.Value?.Content != null)
            {
                using (StreamReader streamReader = new(response.Value.Content))
                {
                    string line = string.Empty;
                    while ((line = streamReader.ReadLine()!) != null)
                    {
                        logs.Add(line);
                    }
                }
            }
            return logs;
        }

        public async Task<List<string>> GetNames(EContainerName containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName.ToString().ToLower());
            await containerClient.CreateIfNotExistsAsync();
            var blobs = containerClient.GetBlobs();
            List<string> names = new();
            foreach (var blob in blobs)
            {
                names.Add(blob.Name);
            }
            return names;
        }
    }
}
