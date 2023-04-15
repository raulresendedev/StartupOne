using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartupOne.Models
{
    public class EventosMarcados
    {
        public int IdEventoMarcado { get; set; }
        [Required]
        [Column("CD_USUARIO")]
        public int IdUsuario { get; set; }
        [NotMapped]
        public Usuario? Usuario { get; set; }
        public string? Nome { get; set; }
        [Required]
        public DateTime Inicio { get; set; }
        [Required]
        public DateTime Fim { get; set; }
        [Required]
        public bool Status { get; set; }
        public int? Recorrente { get; set; }
        public int? Categoria { get; set; }


        public EventosMarcados(int idEventoMarcado, int idUsuario, string nome, DateTime inicio, DateTime fim, int? recorrente, int? categoria, bool status)
        {
            if (nome == null)
                nome = "Novo Evento";

            IdEventoMarcado = idEventoMarcado;
            IdUsuario = idUsuario;
            Nome = nome;
            Inicio = inicio;
            Fim = fim;
            Recorrente = recorrente;
            Categoria = categoria;
            Status = status;
        }
    }
}