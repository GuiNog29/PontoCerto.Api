using System.Text.Json.Serialization;

namespace PontoCerto.Application.DTOs
{
    public class DepartamentoDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public required string Nome { get; set; }
        public decimal ValorTotalPago { get; set; }
        public decimal ValorTotalDescontado { get; set; }
        public ICollection<PessoaDto> Pessoas { get; set; } = new List<PessoaDto>();
    }
}
