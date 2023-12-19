using Microsoft.Extensions.Configuration;

namespace ETradeAPI.Persistence
{
    static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new(); // .NET 6 ile gelen bu sınıf json okumamız ıcın
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ETradeAPI.Presentation")); // json farklı dosya yollarındaysa belirtmemiz gerekiyor.
                configurationManager.AddJsonFile("appsettings.json"); // ve jsonu ekliyoruz.

                return configurationManager.GetConnectionString("PostgreSQL");
            }
        }
    }
}
