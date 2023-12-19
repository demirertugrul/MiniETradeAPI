using EntityFile = ETradeAPI.Domain.Entities;
namespace ETradeAPI.Application.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<EntityFile::File>
    {
    }
}
