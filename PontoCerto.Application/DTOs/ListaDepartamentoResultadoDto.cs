namespace PontoCerto.Application.DTOs
{
    public class ListaDepartamentoResultadoDto
    {
        public ICollection<DepartamentoResultadoDto> Departamentos { get; set; } = new List<DepartamentoResultadoDto>();
    }
}
