using PontoCerto.Domain.Entities;
using PontoCerto.Domain.Interfaces;
using PontoCerto.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PontoCerto.Infrastructure.Repositories
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly PontoCertoDbContext _dbContext;

        public DepartamentoRepository(PontoCertoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Departamento> CadastrarDepartamento(Departamento departamento)
        {
            _dbContext.Departamentos.Add(departamento);
            await _dbContext.SaveChangesAsync();
            return departamento;
        }

        public async Task<Departamento?> BuscarDepartamentoPorId(int departamentoId)
        {
            return await _dbContext.Departamentos.Include(d => d.Pessoas)
                                                 .ThenInclude(p => p.RegistrosPonto)
                                                 .FirstOrDefaultAsync(d => d.Id == departamentoId);
        }

        public async Task<IEnumerable<Departamento>> BuscarTodosDepartamentos()
        {
            return await _dbContext.Departamentos.Include(d => d.Pessoas)
                                                 .ThenInclude(p => p.RegistrosPonto)
                                                 .ToListAsync();
        }

        public async Task<Departamento> AtualizarDepartamento(Departamento departamento)
        {
            var local = _dbContext.Set<Departamento>()
                          .Local
                          .FirstOrDefault(entry => entry.Id.Equals(departamento.Id));

            if (local != null)
                _dbContext.Entry(local).State = EntityState.Detached;

            _dbContext.Departamentos.Attach(departamento);
            _dbContext.Entry(departamento).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return departamento;
        }

        public async Task<bool> ExcluirDepartamento(int departamentoId)
        {
            var departamento = await _dbContext.Departamentos.FindAsync(departamentoId);

            if (departamento == null)
                return false;

            _dbContext.Departamentos.Remove(departamento);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
