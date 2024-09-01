using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PontoCerto.Domain.Entities
{
    public class RegistroPonto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan InicioAlmoco { get; set; }
        public TimeSpan FimAlmoco { get; set; }
        public TimeSpan HoraSaida { get; set; }
        public int PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }
    }
}
