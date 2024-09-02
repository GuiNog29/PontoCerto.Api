namespace PontoCerto.Application.DTOs
{
    public class LerArquivosDto
    {
        public string Departamento { get; set; }
        public string MesVigencia { get; set; }
        public string AnoVigencia { get; set; }
        public List<PessoaDto> Pessoas { get; set; }
    }
}
