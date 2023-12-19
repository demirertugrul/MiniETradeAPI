using ETradeAPI.Domain.Entities;
using ETradeAPI.Domain.Entities.Common;
using ETradeAPI.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EntityFile = ETradeAPI.Domain.Entities;

namespace ETradeAPI.Persistence.Contexts
{
    public class ETradeAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public ETradeAPIDbContext(DbContextOptions options) : base(options) /* IoC'de dBContext'i talep ederken belirli configuration
        gelmesini istiyorsak ctor'u vericez. Yani database configuration'ları için options ile */
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EntityFile.File> Files { get; set; } /* DbSet olarak burada aralarinda
        kalitimsal olarak iliskileri olan entity'ler table-per-hierarchy(TPH) davranis modeli ile
        db'de tek bir tablo halinde tasarlanir. Ama Aralarinda kalitimsal olmayan Products,Orders,
        Customers gibi Entity'ler de db'de hepsi icin ayri ayri tablo olusur.*/
        /*Files, InvoiceFiles, ProductImageFiles Files'tan kalitim aldiklari icin tph devreye giriyor ve tek tek tablo yerine birlikte olan bir tapblo olusturuyor. */
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            /*ChangeTracker : Entityler üzerinden yapılan değişikliklerin ya da yeni eklenen verilerin yakalanmasını sağlayan 
             propertydir. Update operationlarında Track edilen veriyi yakalayıp elde etmemizi sağlar. */

            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                // _ https://docs.microsoft.com/tr-tr/dotnet/csharp/fundamentals/functional/discards
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow, /* delete için yazdık. delete yaptıktan sonra added ve modified her türlü çalışıcak böylece
                    silinmiş bir veriye added ve modified yapmaya çalışacak dolıyısıyla discard ile en son bunu çalıştır diyoruz ve bir şey
                    döndürmeyeceğini söylüyoruz. 
                    Client'tan delete yaptiktan sonra bu interceptor'da silinmis olan veriye update ve modified yapacagi icin (en son olan switchz, her turlu calisiyor) hata verecektir. Bu yüzden en son discard(_) ile bitiroyruz. 
                                           */
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
