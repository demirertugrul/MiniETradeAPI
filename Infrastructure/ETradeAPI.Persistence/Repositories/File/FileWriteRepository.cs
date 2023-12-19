using ETradeAPI.Application.Repositories;
using ETradeAPI.Persistence.Contexts;

namespace ETradeAPI.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(ETradeAPIDbContext context) : base(context)
        {
        }
    }
}
