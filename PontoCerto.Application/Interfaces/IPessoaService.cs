using PontoCerto.Application.DTOs.Pessoa;

namespace PontoCerto.Application.Interfaces
{
    public interface IPessoaService
    {
        Task<PessoaDto> CadastrarPessoa(PessoaDto pessoaDto);
        Task<PessoaDto?> BuscarPessoaPorId(int pessoaId);
        Task<PessoaDto> AtualizarPessoa(PessoaDto pessoaDto, int pessoaId);
        Task<bool> ExcluirPessoa(int pessoaId);
        Task<IEnumerable<PessoaDto>> BuscarTodasPessoas();
    }
}
