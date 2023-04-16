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
        public int? Prioridade { get; set; }
        [Required]
        public int TempoEstimado { get; set; }
        [Required]
        public DateTime PeriodoInicio { get; set; }
        [Required]
        public DateTime PeriodoFim { get; set; }
        public int? Categoria { get; set; }
        public bool Status { get; set; }

        public EventosPendentes(int idEventoPendente, int idUsuario, string nome, int? categoria, int? prioridade, bool status, int tempoEstimado, DateTime periodoInicio, DateTime periodoFim)
        {
            if(periodoInicio.Year != 1)
               periodoInicio = new DateTime(0002, 1, 1, periodoInicio.Hour, periodoInicio.Minute, 0);


            if (periodoFim.Year != 1)
                periodoFim = new DateTime(0002, 1, 1, periodoFim.Hour, periodoFim.Minute, 0);


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
