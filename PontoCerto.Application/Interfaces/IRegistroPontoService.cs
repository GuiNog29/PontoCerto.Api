using PontoCerto.Application.DTOs;
using PontoCerto.Domain.Entities;

namespace PontoCerto.Application.Interfaces
{
    public interface IRegistroPontoService
    {
        Task<RegistroPontoDto> CadastrarRegistroPonto(RegistroPontoDto registroPontoDto);
        Task<RegistroPontoDto?> BuscarRegistroPontoPorId(int registroPontoId);
        Task<RegistroPontoDto> AtualizarRegistroPonto(RegistroPontoDto registroPontoDto, int registroPontoId);
        Task<bool> ExcluirRegistroPonto(int registroPontoId);
        Task<IEnumerable<RegistroPontoDto>> BuscarTodosRegistrosPonto();
        Task<IEnumerable<RegistroPontoDto>> BuscarTodosRegistrosPontoPessoa(int pessoaId);
    }
}
