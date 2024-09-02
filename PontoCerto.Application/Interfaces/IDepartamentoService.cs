using PontoCerto.Application.DTOs;

namespace PontoCerto.Application.Interfaces
{
    public interface IDepartamentoService
    {
        Task<DepartamentoDto> CadastrarDepartamento(DepartamentoDto departamento);
        Task<DepartamentoDto?> BuscarDepartamentoPorId(int departamentoId);
        Task<DepartamentoDto> AtualizarDepartamento(DepartamentoDto departamento);
        Task<bool> ExcluirDepartamento(int departamentoId);
        Task<IEnumerable<DepartamentoDto>> BuscarTodosDepartamentos();
        Task<IEnumerable<PessoaDto>> BuscarTodasPessoas(int departamentoId);
        Task<ListaDepartamentoResultadoDto> GerarResultadoDepartamento(List<LerArquivosDto> listaPessoas);
        Task<List<LerArquivosDto>> LerArquivos(string caminhoPasta);
    }
}
