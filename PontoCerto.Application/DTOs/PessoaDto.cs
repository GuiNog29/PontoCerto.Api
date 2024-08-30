using Swashbuckle.AspNetCore.Annotations;

namespace PontoCerto.Application.DTOs
{
    public class PessoaDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public decimal ValorHora { get; set; }
        public int DepartamentoId { get; set; }
        public ICollection<RegistroPontoDto> RegistrosPonto { get; set; } = new List<RegistroPontoDto>();
    }
}
