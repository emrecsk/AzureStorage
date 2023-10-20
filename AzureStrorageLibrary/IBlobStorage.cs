namespace AzureStrorageLibrary
{
    public enum EContainerName{
        pictures,
        pdfs,
        logs
    }
    public interface IBlobStorage
    {
        public string BlobURL { get; }
        Task<bool> UploadBlobAsync(EContainerName containerName, string blobName, Stream stream);
        Task<bool> DeleteBlobAsync(EContainerName containerName, string blobName);
        Task<Stream> DownloadBlobAsync(EContainerName containerName, string blobName);
        Task<IEnumerable<string>> ListBlobsAsync(EContainerName containerName);
        Task SetLog(string message, string blobName);
        Task<List<string>> GetLogAsync(string blobName);
        List<string> GetNames(EContainerName containerName);
    }
}