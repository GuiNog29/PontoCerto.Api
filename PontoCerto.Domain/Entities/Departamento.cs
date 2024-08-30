namespace PontoCerto.Domain.Entities
{
    public class Departamento
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public decimal? ValorTotalPago { get; set; }
        public decimal? ValorTotalDescontado { get; set; }
        public ICollection<Pessoa> Pessoas { get; set; } = new List<Pessoa>();
    }
}
