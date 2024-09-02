namespace PontoCerto.Application.DTOs
{
    public class PessoaResultadoDto
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public double ValorHora { get; set; }
        public double TotalReceber { get; set; }
        public double HorasExtras { get; set; }
        public double HorasDebito { get; set; }
        public int DiasFalta { get; set; }
        public int DiasExtras { get; set; }
        public int DiasTrabalhados { get; set; }
    }
}
