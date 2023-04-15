using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StartupOne.Models
{
    public class EventosPendentes
    {
        public int IdEventoPendente{ get; set; }
        [Required]
        [Column("CD_USUARIO")]
        public int IdUsuario { get; set; }
        [NotMapped]
        public virtual Usuario? Usuario { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public int Prioridade { get; set; }
        [Required]
        public int TempoEstimado { get; set; }
        [Required]
        public DateTime PeriodoInicio { get; set; }
        [Required]
        public DateTime PeriodoFim { get; set; }
        public int? Categoria { get; set; }
        public bool Status { get; set; }

        public EventosPendentes(int idEventoPendente, int idUsuario, string nome, int? categoria, int prioridade, bool status, int tempoEstimado, DateTime periodoInicio, DateTime periodoFim)
        {
            IdEventoPendente = idEventoPendente;
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
