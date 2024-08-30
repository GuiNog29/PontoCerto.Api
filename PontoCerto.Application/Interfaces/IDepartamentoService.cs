using PontoCerto.Application.DTOs;

namespace PontoCerto.Application.Interfaces
{
    public interface IDepartamentoService
    {
        Task<DepartamentoDto> CadastrarDepartamento(DepartamentoDto departamento);
        Task<DepartamentoDto?> BuscarDepartamentoPorId(int departamentoId);
        Task<DepartamentoDto> AtualizarDepartamento(DepartamentoDto departamento, int departamentoId);
        Task<bool> ExcluirDepartamento(int departamentoId);
        Task<IEnumerable<DepartamentoDto>> BuscarTodosDepartamentos();
        Task<DepartamentoResultadoDto> GerarResultadoDepartamento(IEnumerable<PessoaDto> pessoas);
        Task<List<PessoaDto>> LerArquivos(string caminhoPasta);
    }
}
