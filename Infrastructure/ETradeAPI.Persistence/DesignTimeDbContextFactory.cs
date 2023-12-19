using ETradeAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ETradeAPI.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETradeAPIDbContext>
    /*
     MANAGER CONSOLE'dan Migration yapmak için Persistence katmanını seçiyoruz ve startup katmanına NuGet'ten
     EntityFrameworkCore.Design paketini yüklüyoruz çünkü uygulama startuptan başlıyor.  'add-migration' 'mig_1 & update-database'
    */

    /*
     DOTNET CLI'dan Migration yapmak için Bu class'ı DesignTimeDbContextFactory'e Context nesnesini veriyoruz. Böylece Context'in
     ctor'unu override ediyoruz. Çünkü DOTNET CLI'da Class Library projesinde bu nesnenin ctor'una ne gideceğini bilmediği için 
     aşağıdaki işlemleri yapıyoruz. 'dotnet ef migrations add mig_1' & 'dotnet ef database update'  
     */
    {
        public ETradeAPIDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ETradeAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
