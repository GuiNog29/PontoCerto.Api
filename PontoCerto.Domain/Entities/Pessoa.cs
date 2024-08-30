using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PontoCerto.Domain.Entities
{
    public class Pessoa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Nome { get; set; }
        public decimal ValorHora { get; set; }
        public int DepartamentoId { get; set; }
        public Departamento? Departamento{ get; set; }
        public ICollection<RegistroPonto> RegistrosPontos { get; set; } = new List<RegistroPonto>();
    }
}
