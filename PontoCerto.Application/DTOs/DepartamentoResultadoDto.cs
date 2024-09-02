namespace PontoCerto.Application.DTOs
{
    public class DepartamentoResultadoDto
    {
        public string Departamento { get; set; }
        public string MesVigencia { get; set; }
        public string AnoVigencia { get; set; }
        public double TotalPagar { get; set; }
        public double TotalDescontos { get; set; }
        public double TotalExtras { get; set; }
        public List<PessoaResultadoDto> Funcionarios { get; set; } = new List<PessoaResultadoDto>();
    }
}
