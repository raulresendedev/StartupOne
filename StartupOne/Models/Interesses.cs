using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StartupOne.Models
{
    public class Interesses
    {
        public int IdInteresse{ get; set; }
        [Required]
        [Column("CD_USUARIO")]
        public int IdUsuario { get; set; }
        [NotMapped]
        public virtual Usuario? Usuario { get; set; }
        [Required]
        public string Nome { get; set; }
        public int? Prioridade { get; set; }
        [Required]
        public DateTime TempoEstimado { get; set; }
        [Required]
        public DateTime PeriodoInicio { get; set; }
        [Required]
        public DateTime PeriodoFim { get; set; }
        public int? Categoria { get; set; }
        public bool Status { get; set; }

        public Interesses(int idInteresse, int idUsuario, string nome, int? categoria, int? prioridade, bool status, DateTime tempoEstimado, DateTime periodoInicio, DateTime periodoFim)
        {

            IdInteresse = idInteresse;
            IdUsuario = idUsuario;
            Nome = nome;
            Categoria = categoria;
            Prioridade = prioridade;
            Status = status;
            TempoEstimado = tempoEstimado;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }
    }
}
