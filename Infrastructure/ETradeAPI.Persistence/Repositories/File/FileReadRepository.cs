using ETradeAPI.Application.Repositories;
using ETradeAPI.Persistence.Contexts;

namespace ETradeAPI.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(ETradeAPIDbContext context) : base(context)
        {
        }
    }
}
