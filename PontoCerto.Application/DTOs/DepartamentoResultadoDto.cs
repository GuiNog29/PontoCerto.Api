namespace PontoCerto.Application.DTOs
{
    public class DepartamentoResultadoDto
    {
        public ICollection<DepartamentoDto> Departamentos { get; set; } = new List<DepartamentoDto>();
    }
}
