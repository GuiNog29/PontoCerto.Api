namespace PontoCerto.Application.DTOs.Departamento
{
    public class ListaDepartamentoDto
    {
        public ICollection<DepartamentoDto> Departamentos { get; set; } = new List<DepartamentoDto>();
    }
}
