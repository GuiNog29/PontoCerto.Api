using PontoCerto.Domain.Entities;

namespace PontoCerto.Domain.Interfaces
{
    public interface IRegistroPontoRepository
    {
        Task<RegistroPonto> CadastrarRegistroPonto(RegistroPonto registroPonto);
        Task<RegistroPonto?> BuscarRegistroPontoPorId(int registroPontoId);
        Task<bool> ExcluirRegistroPonto(int registroPontoId);
        Task<RegistroPonto> AtualizarPessoa(RegistroPonto registroPonto);
        Task<IEnumerable<RegistroPonto>> BuscarTodosRegistrosPonto(int pessoaId);
    }
}
