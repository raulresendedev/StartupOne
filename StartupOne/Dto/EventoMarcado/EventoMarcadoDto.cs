namespace StartupOne.Dto.EventoMarcado
{
    public class EventoMarcadoDto
    {
        public int IdEventoMarcado { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public string Nome { get; set; }
        public bool Concluido { get; set; }
        public bool Confirmado { get; set; }
        public int? Categoria { get; set; }
    }
}
