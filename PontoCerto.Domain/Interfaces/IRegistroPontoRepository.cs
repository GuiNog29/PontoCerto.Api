using PontoCerto.Domain.Entities;

namespace PontoCerto.Domain.Interfaces
{
    public interface IRegistroPontoRepository
    {
        Task<Pessoa> CadastrarPessoa(Pessoa pessoa);
        Task<Pessoa> BuscarPessoaPorId(int pessoaId);
        Task<IEnumerable<Pessoa>> BuscarTodasPessoas();
    }
}
