using PontoCerto.Domain.Entities;

namespace PontoCerto.Domain.Interfaces
{
    public interface IDepartamentoRepository
    {
        Task<Departamento> CadastrarDepartamento(Departamento departamento);
        Task<Departamento?> BuscarDepartamentoPorId(int departamentoId);
        Task<Departamento> AtualizarDepartamento(Departamento departamento);
        Task<bool> ExcluirDepartamento(int departamentoId);
        Task<IEnumerable<Departamento>> BuscarTodosDepartamentos();
    }
}
