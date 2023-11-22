namespace StartupOne.Dto.Interesses
{
    public class InteressesDto
    {
        public int idInteresse { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime TempoEstimado { get; set; }
        public string Nome { get; set; }
        public bool Status { get; set; }
        public int? Categoria { get; set; }
        public int? Prioridade { get; set; }
    }
}
