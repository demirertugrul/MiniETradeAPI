using EntityFile = ETradeAPI.Domain.Entities;
namespace ETradeAPI.Application.Repositories
{
    public interface IFileReadRepository : IReadRepository<EntityFile.File>
    {
    }
}
