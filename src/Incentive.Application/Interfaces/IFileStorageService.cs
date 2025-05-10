namespace Incentive.Application.Interfaces
{
    /// <summary>
    /// File storage service interface
    /// </summary>
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType);
        Task<Stream> GetFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
        string GetFileUrl(string fileName);
    }
}
