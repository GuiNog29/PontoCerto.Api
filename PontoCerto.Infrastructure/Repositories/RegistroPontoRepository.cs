using PontoCerto.Infrastructure.Data;

namespace PontoCerto.Infrastructure.Repositories
{
    public class RegistroPontoRepository
    {
        private readonly PontoCertoDbContext _dbContext;
        public RegistroPontoRepository(PontoCertoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
