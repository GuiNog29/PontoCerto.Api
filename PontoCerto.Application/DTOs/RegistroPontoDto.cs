using Swashbuckle.AspNetCore.Annotations;

namespace PontoCerto.Application.DTOs
{
    public class RegistroPontoDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSaida { get; set; }
        public TimeSpan HoraAlmoco { get; set; }
        public int PessoaId { get; set; }
        public PessoaDto? Pessoa { get; set; }
    }
}
