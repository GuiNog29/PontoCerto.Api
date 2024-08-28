namespace PontoCerto.Application.DTOs
{
    public class ErroDto
    {
        public required string Mensagem { get; set; }
        public string? Detalhes { get; set; }
    }
}
