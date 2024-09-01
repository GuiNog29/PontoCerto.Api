using AutoMapper;
using PontoCerto.Domain.Entities;
using PontoCerto.Application.DTOs;
using PontoCerto.Domain.Interfaces;
using PontoCerto.Application.Interfaces;

namespace PontoCerto.Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IMapper _mapper;
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IMapper mapper, IPessoaRepository pessoaRepository)
        {
            _mapper = mapper;
            _pessoaRepository = pessoaRepository;
        }

        public async Task<PessoaDto> CadastrarPessoa(PessoaDto pessoaDto)
        {
            ValidarPessoaDto(pessoaDto);
            var pessoa = await _pessoaRepository.CadastrarPessoa(_mapper.Map<Pessoa>(pessoaDto));
            if (pessoa == null)
                throw new PessoaServiceException("Ocorreu um erro ao criar uma nova pessoa.");

            return _mapper.Map<PessoaDto>(pessoa);
        }

        public async Task<PessoaDto?> BuscarPessoaPorId(int pessoaId)
        {
            var pessoa = await _pessoaRepository.BuscarPessoaPorId(pessoaId);
            if (pessoa == null)
                throw new PessoaNotFoundException(pessoaId);

            return _mapper.Map<PessoaDto>(pessoa);
        }

        public async Task<PessoaDto> AtualizarPessoa(PessoaDto pessoaDto, int pessoaId)
        {
            ValidarPessoaDto(pessoaDto, pessoaId);
            var pessoa = _mapper.Map<Pessoa>(pessoaDto);
            pessoa.Id = pessoaId;
            var pessoaAtualizada = await _pessoaRepository.AtualizarPessoa(pessoa);
            if (pessoaAtualizada == null)
                throw new PessoaServiceException($"Ocorreu um erro ao atualizar a pessoa com Id:{pessoaId}.");

            return _mapper.Map<PessoaDto>(pessoaAtualizada);
        }

        public async Task<bool> ExcluirPessoa(int pessoaId)
        {
            await BuscarPessoaPorId(pessoaId);
            return await _pessoaRepository.ExcluirPessoa(pessoaId);
        }

        public async Task<IEnumerable<PessoaDto>> BuscarTodasPessoas(int departamentoId)
        {
            return _mapper.Map<IEnumerable<PessoaDto>>(await _pessoaRepository.BuscarTodasPessoas(departamentoId));
        }

        private async void ValidarPessoaDto(PessoaDto pessoaDto, int pessoaId = 0)
        {
            if (pessoaId > 0)
                await BuscarPessoaPorId(pessoaId);

            if (pessoaDto == null)
                throw new ArgumentNullException(nameof(pessoaDto));

            if(pessoaDto.DepartamentoId <= 0)
                throw new Exception("O valor do departamentoId deve ser maior que 0.");

            if (pessoaDto.ValorHora <= 0)
                throw new PessoaServiceException("O valor da hora do colaborador deve ser maior que 0.");
        }
    }

    public class PessoaNotFoundException : Exception
    {
        public PessoaNotFoundException(int avaliacaoId)
            : base($"Não foi localizada a pessoa com Id:{avaliacaoId}.") { }
    }

    public class PessoaServiceException : Exception
    {
        public PessoaServiceException(string message)
            : base(message) { }
    }
}
