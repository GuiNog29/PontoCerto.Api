﻿using AutoMapper;
using PontoCerto.Domain.Entities;
using PontoCerto.Application.DTOs;
using PontoCerto.Domain.Interfaces;
using PontoCerto.Application.Interfaces;

namespace PontoCerto.Application.Services
{
    public class RegistroPontoService : IRegistroPontoService
    {
        private readonly IMapper _mapper;
        private readonly IRegistroPontoRepository _registroPontoRepository;

        public RegistroPontoService(IMapper mapper, IRegistroPontoRepository registroPontoRepository)
        {
            _mapper = mapper;
            _registroPontoRepository = registroPontoRepository;
        }

        public async Task<RegistroPontoDto> CadastrarRegistroPonto(RegistroPontoDto registroPontoDto)
        {
            ValidarRegistroPontoDto(registroPontoDto);
            var registroPonto = await _registroPontoRepository.CadastrarRegistroPonto(_mapper.Map<RegistroPonto>(registroPontoDto));
            if (registroPonto == null)
                throw new RegistroPontoServiceException("Ocorreu um erro ao criar um novo registro de ponto.");

            return _mapper.Map<RegistroPontoDto>(registroPonto);
        }

        public async Task<RegistroPontoDto?> BuscarRegistroPontoPorId(int registroPontoId)
        {
            var registroPonto = await _registroPontoRepository.BuscarRegistroPontoPorId(registroPontoId);
            if (registroPonto == null)
                throw new RegistroPontoNotFoundException(registroPontoId);

            return _mapper.Map<RegistroPontoDto>(registroPonto);
        }

        public async Task<RegistroPontoDto> AtualizarRegistroPonto(RegistroPontoDto registroPontoDto)
        {
            ValidarRegistroPontoDto(registroPontoDto);
            var registroPonto = _mapper.Map<RegistroPonto>(registroPontoDto);
            var registroPontoAtualizado = await _registroPontoRepository.AtualizarRegistroPonto(registroPonto);
            if (registroPontoAtualizado == null)
                throw new PessoaServiceException($"Ocorreu um erro ao atualizar a pessoa com Id:{registroPontoDto.Id}.");

            return _mapper.Map<RegistroPontoDto>(registroPontoAtualizado);
        }

        public async Task<bool> ExcluirRegistroPonto(int registroPontoId)
        {
            await BuscarRegistroPontoPorId(registroPontoId);
            return await _registroPontoRepository.ExcluirRegistroPonto(registroPontoId);
        }

        public async Task<IEnumerable<RegistroPontoDto>> BuscarTodosRegistrosPontoPessoa(int pessoaId)
        {
            return _mapper.Map<IEnumerable<RegistroPontoDto>>(await _registroPontoRepository.BuscarTodosRegistrosPontoPessoa(pessoaId));
        }

        private async void ValidarRegistroPontoDto(RegistroPontoDto registroPontoDto)
        {
            if (registroPontoDto.Id > 0)
                await BuscarRegistroPontoPorId(registroPontoDto.Id);

            if (registroPontoDto == null)
                throw new ArgumentNullException(nameof(registroPontoDto));

            if(registroPontoDto.PessoaId <= 0)
                throw new RegistroPontoServiceException("O valor da PessoaId deve ser maior que zero.");
        }
    }

    public class RegistroPontoNotFoundException : Exception
    {
        public RegistroPontoNotFoundException(int avaliacaoId)
            : base($"Não foi localizado registro de ponto com Id:{avaliacaoId}.") { }
    }

    public class RegistroPontoServiceException : Exception
    {
        public RegistroPontoServiceException(string message)
            : base(message) { }
    }
}
