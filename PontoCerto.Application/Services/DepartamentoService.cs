using AutoMapper;
using System.Globalization;
using PontoCerto.Domain.Entities;
using PontoCerto.Application.DTOs;
using PontoCerto.Domain.Interfaces;
using PontoCerto.Application.Helpers;
using PontoCerto.Application.Interfaces;
using Microsoft.Win32;

namespace PontoCerto.Application.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IMapper _mapper;
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IPessoaRepository _pessoaRepository;

        public DepartamentoService(IMapper mapper, IDepartamentoRepository departamentoRepository,
                                   IPessoaRepository pessoaRepository)
        {
            _mapper = mapper;
            _departamentoRepository = departamentoRepository;
            _pessoaRepository = pessoaRepository;
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

        public async Task<IEnumerable<PessoaDto>> BuscarTodasPessoas(int departamentoId)
        {
            return _mapper.Map<IEnumerable<PessoaDto>>(await _pessoaRepository.BuscarTodasPessoas(departamentoId));
        }

        public async Task<ListaDepartamentoResultadoDto> GerarResultadoDepartamento(List<LerArquivosDto> lerAquivos)
        {
            return await Task.Run(() =>
            {
                var listaDepartamentoResultado = new ListaDepartamentoResultadoDto();

                foreach (var arquivo in lerAquivos)
                {
                    var departamentoResultado = new DepartamentoResultadoDto
                    {
                        Departamento = arquivo.Departamento,
                        MesVigencia = arquivo.MesVigencia,
                        AnoVigencia = arquivo.AnoVigencia,
                        Funcionarios = new List<PessoaResultadoDto>()
                    };

                    foreach (var pessoa in arquivo.Pessoas)
                    {
                        double totalHorasTrabalhadas = 0;
                        int diasTrabalhados = 0;
                        int diasFalta = 0;
                        int diasExtras = 0;
                        double horasDebito = 0;
                        double horasExtras = 0;
                        var totalDiasUteisDoMes = ContarDiasUteis(arquivo.MesVigencia, arquivo.AnoVigencia);

                        foreach (var registroPonto in pessoa.RegistrosPonto)
                        {
                            var horasDiarias = CalcularHoraDiaria(registroPonto);
                            if (horasDiarias > 0)
                            {
                                diasTrabalhados++;

                                if (horasDiarias < 8)
                                    horasDebito += 8 - horasDiarias;
                                else if (horasDiarias > 8)
                                    horasExtras += horasDiarias - 8;
                            }
                            else
                                diasFalta++;

                            totalHorasTrabalhadas += horasDiarias;
                        }

                        if (diasTrabalhados > totalDiasUteisDoMes)
                            diasExtras += diasTrabalhados - totalDiasUteisDoMes;

                        totalHorasTrabalhadas += horasExtras + horasDebito;
                        var totalReceber = Math.Round(totalHorasTrabalhadas * (double)pessoa.ValorHora, 2);

                        var pessoaResultado = new PessoaResultadoDto
                        {
                            Nome = pessoa.Nome,
                            Codigo = pessoa.Id,
                            ValorHora = Math.Round((double)pessoa.ValorHora, 2),
                            TotalReceber = totalReceber,
                            HorasExtras = Math.Round(horasExtras, 2),
                            HorasDebito = Math.Round(horasDebito, 2),
                            DiasFalta = diasFalta,
                            DiasExtras = diasExtras,
                            DiasTrabalhados = diasTrabalhados
                        };

                        departamentoResultado.Funcionarios.Add(pessoaResultado);
                    }

                    departamentoResultado.TotalPagar = Math.Round(departamentoResultado.Funcionarios.Sum(f => f.TotalReceber), 2);
                    departamentoResultado.TotalDescontos = Math.Round(departamentoResultado.Funcionarios.Sum(f => f.HorasDebito * (double)(arquivo.Pessoas.FirstOrDefault(p => p.Id == f.Codigo)?.ValorHora ?? 0m)), 2);
                    departamentoResultado.TotalExtras = Math.Round(departamentoResultado.Funcionarios.Sum(f => f.HorasExtras * (double)(arquivo.Pessoas.FirstOrDefault(p => p.Id == f.Codigo)?.ValorHora ?? 0m)), 2);

                    listaDepartamentoResultado.Departamentos.Add(departamentoResultado);
                }

                return listaDepartamentoResultado;
            });
        }

        private int ContarDiasUteis(string mes, string ano)
        {
            int diasUteis = 0;
            int numeroMes = DateTime.ParseExact(mes, "MMMM", new CultureInfo("pt-BR")).Month;
            int anoCorreto = int.Parse(ano);
            int diasNoMes = DateTime.DaysInMonth(anoCorreto, numeroMes);

            for (int dia = 1; dia <= diasNoMes; dia++)
            {
                DateTime dataAtual = new DateTime(anoCorreto, numeroMes, dia);

                // Verifica se é sábado ou domingo
                if (dataAtual.DayOfWeek == DayOfWeek.Saturday || dataAtual.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                // Conta como dia útil
                diasUteis++;
            }

            return diasUteis;
        }

        private double CalcularHoraDiaria(RegistroPontoDto registroPontoDto)
        {
            var minutos =
                (ConverterHoras.ConverterHoraSaidaParaTimeSpan(registroPontoDto.HoraSaida).TotalMinutes -
                 ConverterHoras.ConverterHoraEntradaParaTimeSpan(registroPontoDto.HoraEntrada).TotalMinutes) -
                (ConverterHoras.ConverterFimAlmocoParaTimeSpan(registroPontoDto.FimAlmoco).TotalMinutes -
                 ConverterHoras.ConverterInicioAlmocoParaTimeSpan(registroPontoDto.InicioAlmoco).TotalMinutes);

            return minutos / 60; // Converte minutos para horas
        }

        public async Task<List<LerArquivosDto>> LerArquivos(string caminhoPasta)
        {
            var arquivos = Directory.GetFiles(caminhoPasta, "*.csv");
            var listaLerArquivosDto = new List<LerArquivosDto>();

            var tarefas = arquivos.Select(async arquivo =>
            {
                var nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                var partesNome = nomeArquivo.Split('-');

                if (partesNome.Length < 3)
                    throw new DepartamentoServiceException($"Nome de arquivo mal formatado: {nomeArquivo}");

                var departamentoNome = partesNome[0];
                var mesVigencia = partesNome[1];
                var anoVigencia = partesNome[2];

                var lerArquivoDto = new LerArquivosDto
                {
                    Departamento = departamentoNome ?? "Nome no formato incorreto",
                    MesVigencia = mesVigencia ?? "Nome do arquivo no formato incorreto",
                    AnoVigencia = anoVigencia?? "Nome do arquivo no formato incorreto",
                    Pessoas = new List<PessoaDto>()
                };

                var linhas = await File.ReadAllLinesAsync(arquivo);

                foreach (var linha in linhas.Skip(1)) // Pule a primeira linha que contém o cabeçalho
                {
                    if (string.IsNullOrWhiteSpace(linha))
                        continue;

                    var colunas = linha.Split(';');

                    if (colunas.Length < 7)
                        throw new DepartamentoServiceException($"Linha mal formatada ou incompleta: {linha}");

                    var pessoaId = int.Parse(colunas[0]);
                    var nome = colunas[1];
                    var valorHora = decimal.Parse(colunas[2].Replace("R$", "").Trim(), CultureInfo.InvariantCulture);
                    var data = DateTime.ParseExact(colunas[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var entrada = colunas[4];
                    var saida = colunas[5];
                    var almoco = colunas[6];

                    var InicioFimAlmoco = almoco.Split('-');
                    var inicioAlmoco = InicioFimAlmoco[0].Trim().Replace(" ", "");
                    var fimAlmoco = InicioFimAlmoco[1].Trim().Replace(" ", "");

                    // Verifica se a pessoa já existe na lista de pessoas do departamento
                    var pessoa = lerArquivoDto.Pessoas.FirstOrDefault(p => p.Id == pessoaId);

                    if (pessoa == null)
                    {
                        // Se a pessoa não existir, cria uma nova
                        pessoa = new PessoaDto
                        {
                            Id = pessoaId,
                            Nome = nome,
                            ValorHora = valorHora,
                            RegistrosPonto = new List<RegistroPontoDto>()
                        };
                        lerArquivoDto.Pessoas.Add(pessoa);
                    }

                    // Adiciona o novo registro de ponto à lista de registros da pessoa
                    var registroPonto = new RegistroPontoDto
                    {
                        Data = data.ToString("dd/MM/yyyy"),
                        HoraEntrada = entrada,
                        Almoco = almoco,
                        HoraSaida = saida,
                        InicioAlmoco = inicioAlmoco,
                        FimAlmoco = fimAlmoco,
                        Pessoa = pessoa
                    };

                    pessoa.RegistrosPonto.Add(registroPonto);
                }

                listaLerArquivosDto.Add(lerArquivoDto);

            }).ToList();

            await Task.WhenAll(tarefas);

            return listaLerArquivosDto;
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
