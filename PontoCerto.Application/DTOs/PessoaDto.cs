using Swashbuckle.AspNetCore.Annotations;

namespace PontoCerto.Application.DTOs
{
    public class PessoaDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        public required string Nome { get; set; }
        public decimal ValorHora { get; set; }
        public ICollection<RegistroPontoDto> RegistrosPonto { get; set; } = new List<RegistroPontoDto>();
    }
}
