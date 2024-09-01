namespace PontoCerto.Application.DTOs
{
    public class RegistroPontoDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan InicioAlmoco { get; set; }
        public TimeSpan FimAlmoco { get; set; }
        public TimeSpan HoraSaida { get; set; }
        public int PessoaId { get; set; }
        public required PessoaDto Pessoa { get; set; }

        public string Almoco
        {
            get
            {
                return $"{InicioAlmoco:hh\\:mm} - {FimAlmoco:hh\\:mm}";
            }
        }
    }
}
