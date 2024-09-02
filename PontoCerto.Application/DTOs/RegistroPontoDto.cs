using System.Text.Json.Serialization;

namespace PontoCerto.Application.DTOs
{
    public class RegistroPontoDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public required string Data { get; set; }
        public required string HoraEntrada { get; set; }
        public string? InicioAlmoco { get; set; }
        public string? FimAlmoco { get; set; }
        public required string HoraSaida { get; set; }
        public int PessoaId { get; set; }
        public PessoaDto? Pessoa { get; set; }

        public string Almoco
        {
            get
            {
                if (string.IsNullOrEmpty(InicioAlmoco) || string.IsNullOrEmpty(FimAlmoco))
                    return "Sem informação de almoço";

                return $"{TimeSpan.Parse(InicioAlmoco):hh\\:mm} – {TimeSpan.Parse(FimAlmoco):hh\\:mm}";
            }
            set { }
        }
    }
}
