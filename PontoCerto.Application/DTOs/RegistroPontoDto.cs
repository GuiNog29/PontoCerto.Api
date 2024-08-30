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
        public required string Almoco { get; set; }
        public int PessoaId { get; set; }
        public required PessoaDto Pessoa { get; set; }

        public TimeSpan DuracaoAlmoco
        {
            get
            {
                var horasAlmoco = Almoco.Split('-');
                var horaSaidaAlmoco = TimeSpan.Parse(horasAlmoco[0].Trim());
                var horaRetornoAlmoco = TimeSpan.Parse(horasAlmoco[1].Trim());
                return horaRetornoAlmoco - horaSaidaAlmoco;
            }
        }
    }
}
