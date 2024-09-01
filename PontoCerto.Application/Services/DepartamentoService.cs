using AutoMapper;
using System.Globalization;
using PontoCerto.Domain.Entities;
using PontoCerto.Application.DTOs;
using PontoCerto.Domain.Interfaces;
using PontoCerto.Application.Interfaces;

namespace PontoCerto.Application.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IMapper _mapper;
        private readonly IDepartamentoRepository _departamentoRepository;

        public DepartamentoService(IMapper mapper, IDepartamentoRepository departamentoRepository)
        {
            _mapper = mapper;
            _departamentoRepository = departamentoRepository;
        }

        public async Task<DepartamentoDto> CadastrarDepartamento(DepartamentoDto departamentoDto)
        {
            ValidarDepartamentoDto(departamentoDto);
            var departamento = await _departamentoRepository.CadastrarDepartamento(_mapper.Map<Departamento>(departamentoDto));
            if (departamento == null)
                throw new DepartamentoServiceException("Ocorreu um erro ao criar um novo departamento.");

            return _mapper.Map<DepartamentoDto>(departamento);
        }

        public async Task<DepartamentoDto?> BuscarDepartamentoPorId(int departamentoId)
        {
            var departamento = await _departamentoRepository.BuscarDepartamentoPorId(departamentoId);
            if (departamento == null)
                throw new DepartamentoNotFoundException(departamentoId);

            return _mapper.Map<DepartamentoDto>(departamento);
        }

        public async Task<DepartamentoDto> AtualizarDepartamento(DepartamentoDto departamentoDto)
        {
            ValidarDepartamentoDto(departamentoDto);
            var departamento = _mapper.Map<Departamento>(departamentoDto);
            var departamentoAtualizado = await _departamentoRepository.AtualizarDepartamento(departamento);
            if (departamentoAtualizado == null)
                throw new DepartamentoServiceException($"Ocorreu um erro ao atualizar o departamento com Id:{departamentoDto.Id}.");

            return _mapper.Map<DepartamentoDto>(departamentoAtualizado);
        }

        public async Task<bool> ExcluirDepartamento(int departamentoId)
        {
            await BuscarDepartamentoPorId(departamentoId);
            return await _departamentoRepository.ExcluirDepartamento(departamentoId);
        }

        public async Task<IEnumerable<DepartamentoDto>> BuscarTodosDepartamentos()
        {
            return _mapper.Map<IEnumerable<DepartamentoDto>>(await _departamentoRepository.BuscarTodosDepartamentos());
        }

        public async Task<DepartamentoResultadoDto> GerarResultadoDepartamento(IEnumerable<PessoaDto> pessoas)
        {
            return await Task.Run(() =>
            {
                var departamentoResultado = new DepartamentoResultadoDto();

                var departamentos = pessoas
                    .SelectMany(p => p.RegistrosPonto, (p, r) => new { Pessoa = p, Registro = r })
                    .GroupBy(x => new { x.Registro.Data.Year, x.Registro.Data.Month })
                    .Select(g => new DepartamentoDto
                    {
                        Nome = $"Departamento-{g.Key.Month}-{g.Key.Year}",
                        Pessoas = g
                            .GroupBy(x => x.Pessoa.Id)
                            .Select(pg =>
                            {
                                var pessoaDto = pg.Key;
                                var pessoa = pg.First().Pessoa;
                                pessoa.RegistrosPonto = pg.Select(x => x.Registro).ToList();
                                return pessoa;
                            })
                            .ToList()
                    })
                    .ToList();

                foreach (var departamento in departamentos)
                {
                    foreach (var pessoa in departamento.Pessoas)
                    {
                        foreach (var registro in pessoa.RegistrosPonto)
                        {
                            var horasTrabalhadas = (registro.HoraSaida - registro.HoraEntrada) - registro.DuracaoAlmoco;
                            if (horasTrabalhadas.TotalHours < 8)
                            {
                                departamento.ValorTotalDescontado += (decimal)(8 - horasTrabalhadas.TotalHours) * pessoa.ValorHora;
                            }
                            else if (horasTrabalhadas.TotalHours > 8)
                            {
                                departamento.ValorTotalPago += (decimal)(horasTrabalhadas.TotalHours - 8) * pessoa.ValorHora;
                            }

                            departamento.ValorTotalPago += 8 * pessoa.ValorHora;
                        }
                    }

                    departamentoResultado.Departamentos.Add(departamento);
                }

                return departamentoResultado;
            });
        }

        public async Task<List<PessoaDto>> LerArquivos(string caminhoPasta)
        {
            var arquivos = Directory.GetFiles(caminhoPasta, "*.csv");
            var colaboradores = new Dictionary<int, PessoaDto>();

            var tarefas = arquivos.Select(async arquivo =>
            {
                var linhas = await File.ReadAllLinesAsync(arquivo);
                foreach (var linha in linhas.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(linha))
                        continue;

                    var colunas = linha.Split(';');

                    try
                    {
                        var pessoaId = int.Parse(colunas[0]);
                        var nome = colunas[1];
                        var valorHora = decimal.Parse(colunas[2], CultureInfo.InvariantCulture);
                        var data = DateTime.ParseExact(colunas[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var entrada = TimeSpan.Parse(colunas[4]);
                        var saida = TimeSpan.Parse(colunas[5]);
                        var almoco = colunas[6].Trim();

                        lock (colaboradores)
                        {
                            if (!colaboradores.TryGetValue(pessoaId, out var pessoa))
                            {
                                pessoa = new PessoaDto
                                {
                                    Id = pessoaId,
                                    Nome = nome,
                                    ValorHora = valorHora,
                                    RegistrosPonto = new List<RegistroPontoDto>()
                                };
                                colaboradores[pessoaId] = pessoa;
                            }

                            var registroPonto = new RegistroPontoDto
                            {
                                Data = data,
                                HoraEntrada = entrada,
                                HoraSaida = saida,
                                Almoco = almoco,
                                Pessoa = pessoa
                            };

                            pessoa.RegistrosPonto.Add(registroPonto);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar linha: {linha}. Exceção: {ex.Message}");
                        throw;
                    }
                };
            });

            await Task.WhenAll(tarefas);

            return colaboradores.Values.ToList();
        }

        private async void ValidarDepartamentoDto(DepartamentoDto departamentoDto)
        {
            if (departamentoDto.Id > 0)
                await BuscarDepartamentoPorId(departamentoDto.Id);

            if (departamentoDto == null)
                throw new ArgumentNullException(nameof(departamentoDto));

            if (string.IsNullOrWhiteSpace(departamentoDto.Nome))
                throw new DepartamentoServiceException("O nome do departamento é obrigatório.");
        }
    }

    public class DepartamentoNotFoundException : Exception
    {
        public DepartamentoNotFoundException(int departamentoId)
            : base($"Não foi localizado o departamento com Id:{departamentoId}.") { }
    }

    public class DepartamentoServiceException : Exception
    {
        public DepartamentoServiceException(string message)
            : base(message) { }
    }
}
