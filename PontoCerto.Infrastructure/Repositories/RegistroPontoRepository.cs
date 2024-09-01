using PontoCerto.Domain.Entities;
using PontoCerto.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PontoCerto.Infrastructure.Data;

namespace PontoCerto.Infrastructure.Repositories
{
    public class RegistroPontoRepository : IRegistroPontoRepository
    {
        private readonly PontoCertoDbContext _dbContext;
        public RegistroPontoRepository(PontoCertoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RegistroPonto> CadastrarRegistroPonto(RegistroPonto registroPonto)
        {
            _dbContext.RegistrosPontos.Add(registroPonto);
            await _dbContext.SaveChangesAsync();
            return registroPonto;
        }

        public async Task<RegistroPonto?> BuscarRegistroPontoPorId(int registroPontoId)
        {
            return await _dbContext.RegistrosPontos.Include(x => x.Pessoa)
                                           .FirstOrDefaultAsync(x => x.Id == registroPontoId);
        }

        public async Task<RegistroPonto> AtualizarRegistroPonto(RegistroPonto registroPonto)
        {
            var local = _dbContext.Set<RegistroPonto>()
                          .Local
                          .FirstOrDefault(entry => entry.Id.Equals(registroPonto.Id));

            if (local != null)
                _dbContext.Entry(local).State = EntityState.Detached;

            _dbContext.RegistrosPontos.Attach(registroPonto);
            _dbContext.Entry(registroPonto).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return registroPonto;
        }

        public async Task<bool> ExcluirRegistroPonto(int registroPontoId)
        {
            var registroPonto = await _dbContext.RegistrosPontos.FindAsync(registroPontoId);

            if (registroPonto == null)
                return false;

            _dbContext.RegistrosPontos.Remove(registroPonto);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RegistroPonto>> BuscarTodosRegistrosPontoPessoa(int pessoaId)
        {
            return await _dbContext.RegistrosPontos.Where(x => x.PessoaId == pessoaId).Include(x => x.Pessoa).ToListAsync();
        }
    }
}
