using StartupOne.Models;
using StartupOne.Repository;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace StartupOne.Service
{
    public class EventosPendentesService
    {
        private readonly EventosPendentesRepository _eventosPendentesRepository = new();
        private readonly EventoMarcadoRepository _eventosMarcadosRepository = new();
        DateTime dataAtual = DateTime.Now.Date;

        public void ValidarEventoPendente(EventosPendentes eventoPendente)
        {
            if (eventoPendente.Prioridade == null)
                throw new Exception("Campo Prioridade é obrigatório");

            if (eventoPendente.PeriodoInicio.Year == 1)
                throw new Exception("Campo periodo início é obrigatório");

            if (eventoPendente.PeriodoFim.Year == 1)
                throw new Exception("Campo periodo fim é obrigatório");

            if (eventoPendente.Prioridade > 2 || eventoPendente.Prioridade < 0)
                throw new Exception("Campo Prioridade inválido");

            if (eventoPendente.TempoEstimado < 5)
                throw new Exception("Campo de tempo estimado deve ter o mínimo de 5");

            if (eventoPendente.TempoEstimado > 1440)
                throw new Exception("Campo de tempo estimado deve ter o máximo de 1440");

            if (eventoPendente.PeriodoInicio == eventoPendente.PeriodoFim)
                throw new Exception("Periodo fim e início não podem ser iguais.");

            if (eventoPendente.PeriodoFim < eventoPendente.PeriodoInicio)
                throw new Exception("Periodo fim não pode ser menor do que periodo início.");

            if ((eventoPendente.PeriodoFim.TimeOfDay.TotalMinutes - eventoPendente.PeriodoInicio.TimeOfDay.TotalMinutes) * 1 < eventoPendente.TempoEstimado)
                throw new Exception("Tempo estimado não pode ser maior que o período");

            //if (eventoPendente.PeriodoInicio.Minute % 5 != 0 || eventoPendente.PeriodoFim.Minute % 5 != 0)
            //    throw new Exception("Os horários de início e fim devem ser múltiplos de 5.");

            if (eventoPendente.TempoEstimado % 5 != 0 || eventoPendente.TempoEstimado % 5 != 0)
                throw new Exception("O tempo estimado deve ser múltiplo de 5");

            if (eventoPendente.Status != true)
                eventoPendente.Status = true;

        }

        public void CadastrarEvento(EventosPendentes evento)
        {
            ValidarEventoPendente(evento);

            _eventosPendentesRepository.Adicionar(evento);
        }

        public void AtualizarEvento(EventosPendentes evento)
        {
            ValidarEventoPendente(evento);

            _eventosPendentesRepository.Atualizar(evento);
        }

        public void DeletarEvento(int idEvento)
        {
            var evento = _eventosPendentesRepository.Obter(idEvento);
            _eventosPendentesRepository.Remover(evento);
        }

        public EventosPendentes ObterEvento(int idEvento)
        {
            return _eventosPendentesRepository.Obter(idEvento);
        }

        public ICollection<EventosPendentes> ObterTodosEventos(int idUsuario)
        {
            return _eventosPendentesRepository.ObterEventosPendentesDoUsuario(idUsuario);
        }

        public string EncontrarHorarios(int idUsuario)
        {
            bool todosEventosAlocados = false;
            int i = 0;
            List<(string, int)>[] lists = new List<(string, int)>[30]; //DEBUGAR APENAS  
            ICollection<EventoMarcado> eventosMarcadosNoDia = new List<EventoMarcado>();
            List<(string, int)> temposDoDia = new List<(string, int)>();
            ICollection<EventosPendentes> eventosPendentes = _eventosPendentesRepository
                                                                .ObterEventosPendentesDoUsuario(idUsuario)
                                                                .OrderBy(x => x.Prioridade)
                                                                .Where(x => x.Status == true)
                                                                .ToList();

            if (eventosPendentes.Count == 0)
                return "não há eventos pendentes";

            while (!todosEventosAlocados)
            {
                i++;

                dataAtual = dataAtual.AddDays(i);

                var eventosNaoAlocados = eventosPendentes.Where(e => e.Status == true).OrderBy(x => x.TempoEstimado).ToList();

                foreach (var eventoPendente in eventosNaoAlocados)
                {
                    eventosMarcadosNoDia = _eventosMarcadosRepository.ObterEventosFuturosDoUsuario(idUsuario)
                                                                .Where(x => x.Inicio.DayOfYear == dataAtual.DayOfYear)
                                                                .OrderBy(x => x.Inicio)
                                                                .ToList();

                    int intervalo = eventoPendente.TempoEstimado == 1440 ? 0 : 10;
                    
                    temposDoDia = ObterTemposDoDia(eventosMarcadosNoDia);
                    temposDoDia = AtualizarperiodoPermitido(temposDoDia, eventoPendente);
                    foreach (var tempo in temposDoDia)
                    {
                        if (eventoPendente.TempoEstimado + intervalo <= tempo.Item2 && tempo.Item1 == "livre")
                        {
                            AlocarEventoPendente(temposDoDia, tempo, eventoPendente);
                            break;
                        }
                    }
                }
                todosEventosAlocados = eventosPendentes.All(e => e.Status == false);
                lists[i] = temposDoDia;
            }
            return "Ok";
        }
        public void AlocarEventoPendente(List<(string, int)> temposDoDia, (string, int) tempo, EventosPendentes eventoPendente)
        {
            var index = temposDoDia.IndexOf(tempo);

            int tempoEvento = eventoPendente.TempoEstimado;

            int tempoRestante = (tempo.Item2 - tempoEvento) / 2;

            if (tempoRestante == 0)
            {
                temposDoDia[index] = ("ocupado", tempoEvento);
            }
            else
            {
                temposDoDia.Insert(index, ("livre", tempoRestante));

                temposDoDia[index + 1] = ("ocupado", tempoEvento);

                temposDoDia.Insert(index + 2, ("livre", tempoRestante));
            }

            int minutosInicio = 0;

            for (int j = 0; j <= index; j++)
            {
                minutosInicio += temposDoDia[j].Item2;
            }

            int minutosFim = minutosInicio + temposDoDia[index + 1].Item2;

            DateTime dataZerada = new DateTime();
            dataZerada = dataAtual;
            dataZerada = dataZerada.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0);

            DateTime resultadoInicio = dataZerada.AddMinutes(minutosInicio);
            DateTime resultadoFim = dataZerada.AddMinutes(minutosFim);


            eventoPendente.Status = false;

            _eventosPendentesRepository.Atualizar(eventoPendente);

            EventoMarcado alocarEvento = new EventoMarcado(
                    0,
                    eventoPendente.IdUsuario,
                    eventoPendente.Nome,
                    resultadoInicio,
                    resultadoFim,
                    null,
                    null,
                    false,
                    false
                );

            _eventosMarcadosRepository.Adicionar(alocarEvento);
        }
        public List<(string, int)> ObterTemposDoDia(ICollection<EventoMarcado> eventosMarcados)
        {
            List<(string, int)> listaDeTuplas = new List<(string, int)>();

            TimeSpan fimDoEventoAnterior = new TimeSpan();

            foreach (var evento in eventosMarcados)
            {

                TimeSpan inicio;

                if (fimDoEventoAnterior.Ticks == 0)
                {
                    inicio = evento.Inicio.TimeOfDay;
                }
                else
                {
                    inicio = evento.Inicio.TimeOfDay - fimDoEventoAnterior;
                }

                int minutosLivres = (int)inicio.TotalMinutes;

                TimeSpan duracao = (TimeSpan)(evento.Fim - evento.Inicio);
                int duracaoEmMinutos = (int)duracao.TotalMinutes;

                fimDoEventoAnterior = evento.Fim.TimeOfDay;

                listaDeTuplas.Add(("livre", minutosLivres));
                listaDeTuplas.Add(("ocupado", duracaoEmMinutos));
            }
            int totalMinutos = listaDeTuplas.Sum(x => x.Item2);

            if (totalMinutos < 1440)
            {
                listaDeTuplas.Add(("livre", 1440 - totalMinutos));
            }

            return listaDeTuplas;
        }
        public List<(string, int)> AtualizarperiodoPermitido(List<(string, int)> temposDoDia, EventosPendentes eventoPendente)
        {
            var periodoInicioMinutos = eventoPendente.PeriodoInicio.TimeOfDay.TotalMinutes;
            var periodoFimMinutos = eventoPendente.PeriodoFim.TimeOfDay.TotalMinutes;

            var minutos = 0;
            int i = -1;

            if (periodoInicioMinutos != 0)
            {

                while (periodoInicioMinutos > minutos)
                {
                    i++;
                    minutos += temposDoDia[i].Item2;
                }

                if (temposDoDia[i].Item1 == "livre")
                {
                    int minutosLivreRestantes = (int)(minutos - periodoInicioMinutos);
                    temposDoDia.RemoveRange(0, i + 1);
                    temposDoDia.Insert(0, ("livre", minutosLivreRestantes));
                    temposDoDia.Insert(0, ((string, int))("ocupado", periodoInicioMinutos));
                }
                else
                {
                    int minutosOcupadosRestantes = (int)(minutos - periodoInicioMinutos);
                    temposDoDia.RemoveRange(0, i + 1);
                    temposDoDia.Insert(0, ((string, int))("ocupado", periodoInicioMinutos + minutosOcupadosRestantes));
                }
            }

            if (periodoFimMinutos != 0)
            {
                var meiaNoiteMinutos = 1440;
                minutos = 0;
                i = temposDoDia.Count;
                while (meiaNoiteMinutos - periodoFimMinutos >= minutos && i > 0)
                {
                    i--;
                    minutos += temposDoDia[i].Item2;
                }

                if (temposDoDia[i].Item1 == "livre")
                {
                    int minutosLivreRestantes = (int)(minutos - temposDoDia[i].Item2);
                    temposDoDia.RemoveRange(i, temposDoDia.Count - i);
                    temposDoDia.Add(((string, int))("livre", minutos - (meiaNoiteMinutos - periodoFimMinutos)));
                    temposDoDia.Add(((string, int))("ocupado", meiaNoiteMinutos - periodoFimMinutos));
                }
                else
                {
                    temposDoDia.RemoveRange(i, temposDoDia.Count - i);
                    temposDoDia.Add(((string, int))("ocupado", minutos));
                }
            }
            return temposDoDia;
        }
    }
}