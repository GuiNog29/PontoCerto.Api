using PontoCerto.Application.DTOs;

namespace PontoCerto.Application.Interfaces
{
    public interface IRegistroPontoService
    {
        Task<RegistroPontoDto> CadastrarRegistroPonto(RegistroPontoDto registroPontoDto);
        Task<RegistroPontoDto?> BuscarRegistroPontoPorId(int registroPontoId);
        Task<RegistroPontoDto> AtualizarRegistroPonto(RegistroPontoDto registroPontoDto);
        Task<bool> ExcluirRegistroPonto(int registroPontoId);
        Task<IEnumerable<RegistroPontoDto>> BuscarTodosRegistrosPontoPessoa(int pessoaId);
    }
}
