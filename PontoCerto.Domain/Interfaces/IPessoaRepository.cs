using PontoCerto.Domain.Entities;

namespace PontoCerto.Domain.Interfaces
{
    public interface IPessoaRepository
    {
        Task<Pessoa> CadastrarPessoa(Pessoa pessoa);
        Task<Pessoa?> BuscarPessoaPorId(int pessoaId);
        Task<bool> ExcluirPessoa(int pessoaId);
        Task<Pessoa> AtualizarPessoa(Pessoa pessoa);
        Task<IEnumerable<Pessoa>> BuscarTodasPessoas();
    }
}
