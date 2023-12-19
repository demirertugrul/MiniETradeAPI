namespace ETradeAPI.Application.Abstractions.Storage
{
    public interface IStorageService : IStorage /* bu local aws azure gibi storagelerden hangisi isimize
                                                 yarayacaksa dependecy inversion yontemi ile o storageleri
                                                 kullanmak olacaktir.*/
    {
        public string StorageName { get; } // aws azure local 'i  GetType().Name ile string degeri aldik
    }
}
