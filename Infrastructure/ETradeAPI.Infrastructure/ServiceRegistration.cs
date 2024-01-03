using ETradeAPI.Application.Abstractions.Services;
using ETradeAPI.Application.Abstractions.Services.Configurations;
using ETradeAPI.Application.Abstractions.Storage;
using ETradeAPI.Application.Abstractions.Token;
using ETradeAPI.Infrastructure.Enums;
using ETradeAPI.Infrastructure.Services;
using ETradeAPI.Infrastructure.Services.Configurations;
using ETradeAPI.Infrastructure.Services.Storage;
using ETradeAPI.Infrastructure.Services.Storage.Azure;
using ETradeAPI.Infrastructure.Services.Storage.Local;
using ETradeAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace ETradeAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            serviceCollection.AddScoped<IMailService, MailService>();
            serviceCollection.AddScoped<IApplicationService, ApplicationService>();
            serviceCollection.AddScoped<IQRCodeService, QRCodeService>();
        }
        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }
        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:

                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
