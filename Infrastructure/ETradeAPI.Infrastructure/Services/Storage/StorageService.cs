using ETradeAPI.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;

namespace ETradeAPI.Infrastructure.Services.Storage
{
    //StorageService Client'in yani Developer'in kullanacagi service
    public class StorageService : IStorageService // bu aws azure local storage'lere depdency inversion gorevi goruyor.
    {
        readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName => _storage.GetType().Name;

        public Task DeleteAsync(string pathOrContainerName, string fileName)
            => _storage.DeleteAsync(pathOrContainerName, fileName);
        public List<string> GetFiles(string pathOrContainerName)
            => _storage.GetFiles(pathOrContainerName);

        public bool HasFile(string pathOrContainerName, string fileName)
            => _storage.HasFile(pathOrContainerName, fileName);
        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
            => _storage.UploadAsync(pathOrContainerName, files);
    }
}
