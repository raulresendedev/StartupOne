namespace StartupOne.Dto.EventoMarcado
{
    public class EventoMarcadoDto
    {
        public int IdEventoMarcado { get; set; }
        public string Inicio { get; set; }
        public string Fim { get; set; }
        public string Nome { get; set; }
        public bool Status { get; set; }
        public int? Categoria { get; set; }
    }
}
