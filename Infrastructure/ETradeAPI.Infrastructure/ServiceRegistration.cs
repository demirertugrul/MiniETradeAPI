using ETradeAPI.Application.Abstractions.Storage;
using ETradeAPI.Infrastructure.Enums;
using ETradeAPI.Infrastructure.Services.Storage;
using ETradeAPI.Infrastructure.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;

namespace ETradeAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            //services.AddScoped<IFileService, FileService>();
            services.AddScoped<IStorageService, StorageService>();
        }
        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }
        public static void AddStorage(this IServiceCollection services, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.AWS:
                    //services.AddScoped<IStorage, AWSStorage>();
                    break;
                case StorageType.Azure:
                    //services.AddScoped<IStorage, AzureStorage>();
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
