namespace PontoCerto.Application.DTOs
{
    public class RegistroPontoDto
    {
        public int Id { get; set; }
        public required string Data { get; set; }
        public required string HoraEntrada { get; set; }
        public required string InicioAlmoco { get; set; }
        public required string FimAlmoco { get; set; }
        public required string HoraSaida { get; set; }
        public int PessoaId { get; set; }
        public PessoaDto? Pessoa { get; set; }

        public DateTime ConverterDataParaDateTime()
        {
            return DateTime.ParseExact(Data, "dd/MM/yyyy", null);
        }

        public TimeSpan ConverterHoraEntradaParaTimeSpan()
        {
            return TimeSpan.ParseExact(HoraEntrada, "hh\\:mm", null);
        }

        public TimeSpan ConverterInicioAlmocoParaTimeSpan()
        {
            return TimeSpan.ParseExact(InicioAlmoco, "hh\\:mm", null);
        }

        public TimeSpan ConverterFimAlmocoParaTimeSpan()
        {
            return TimeSpan.ParseExact(FimAlmoco, "hh\\:mm", null);
        }

        public TimeSpan ConverterHoraSaidaParaTimeSpan()
        {
            return TimeSpan.ParseExact(HoraSaida, "hh\\:mm", null);
        }

        public string Almoco
        {
            get
            {
                return $"{TimeSpan.Parse(InicioAlmoco):hh\\:mm} - {TimeSpan.Parse(FimAlmoco):hh\\:mm}";
            }
        }

    }
}
