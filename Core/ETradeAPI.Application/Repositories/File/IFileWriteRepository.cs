using F = ETradeAPI.Domain.Entities;

namespace ETradeAPI.Application.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<F::File>
    {
    }
}
