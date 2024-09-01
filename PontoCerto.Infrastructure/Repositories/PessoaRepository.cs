using PontoCerto.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PontoCerto.Domain.Interfaces;
using PontoCerto.Infrastructure.Data;

namespace PontoCerto.Infrastructure.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly PontoCertoDbContext _dbContext;
        public PessoaRepository(PontoCertoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Pessoa> CadastrarPessoa(Pessoa pessoa)
        {
            _dbContext.Pessoas.Add(pessoa);
            await _dbContext.SaveChangesAsync();
            return pessoa;
        }

        public async Task<Pessoa?> BuscarPessoaPorId(int pessoaId)
        {
            return await _dbContext.Pessoas.Include(x => x.RegistrosPontos)
                                           .FirstOrDefaultAsync(x => x.Id == pessoaId);
        }

        public async Task<Pessoa> AtualizarPessoa(Pessoa pessoa)
        {
            var local = _dbContext.Set<Pessoa>()
                          .Local
                          .FirstOrDefault(entry => entry.Id.Equals(pessoa.Id));

            if (local != null)
                _dbContext.Entry(local).State = EntityState.Detached;

            _dbContext.Pessoas.Attach(pessoa);
            _dbContext.Entry(pessoa).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return pessoa;
        }

        public async Task<bool> ExcluirPessoa(int pessoaId)
        {
            var pessoa = await _dbContext.Pessoas.FindAsync(pessoaId);

            if(pessoa == null)
                return false;

            _dbContext.Pessoas.Remove(pessoa);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Pessoa>> BuscarTodasPessoas()
        {
            return await _dbContext.Pessoas.Include(x => x.RegistrosPontos).ToListAsync();
        }
    }
}
