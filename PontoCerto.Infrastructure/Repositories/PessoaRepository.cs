using PontoCerto.Infrastructure.Data;

namespace PontoCerto.Infrastructure.Repositories
{
    public class PessoaRepository
    {
        private readonly PontoCertoDbContext _dbContext;
        public PessoaRepository(PontoCertoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
