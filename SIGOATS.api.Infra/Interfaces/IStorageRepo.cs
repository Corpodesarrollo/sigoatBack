namespace SIGOATS.api.Infra.Interfaces
{
    public interface IStorageRepo
    {
        Task<bool> DeleteFileAsync(string fileName);
        Task<byte[]?> DownloadFileAsync(string fileName);
        Task<List<string>> ListFilesAsync();
        Task<bool> UpdateFileAsync(byte[] fileBytes, string fileName);
        Task<bool> UploadFileAsync(byte[] fileBytes, string fileName, bool replace = false);
    }
}
