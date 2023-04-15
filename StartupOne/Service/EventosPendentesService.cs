using StartupOne.Models;
using StartupOne.Repository;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace StartupOne.Service
{
    public class EventosPendentesService
    {
        private readonly EventosPendentesRepository _eventosPendentesRepository = new();
        private readonly EventosMarcadosRepository _eventosMarcadosRepository = new();
        DateTime dataAtual = DateTime.Now.Date;

        public void CadastrarEvento(EventosPendentes evento)
        {
            _eventosPendentesRepository.Adicionar(evento);
        }

        public void AtualizarEvento(EventosPendentes evento)
        {
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

        public void EncontrarHorarios(int idUsuario)
        {
            ICollection<EventosPendentes> eventosPendentes = _eventosPendentesRepository
                                                                .ObterEventosPendentesDoUsuario(idUsuario)
                                                                .OrderBy(x => x.Prioridade)
                                                                .ToList();

            List<(string, int)>[] lists = new List<(string, int)>[30];

            bool todosEventosAlocados = false;
            
            int i = 0;

            while (!todosEventosAlocados)
            {
                i++;

                dataAtual = dataAtual.AddDays(i);
                
                ICollection<EventosMarcados> eventosMarcados = _eventosMarcadosRepository
                                                                .ObterEventosMarcadosDoUsuario(idUsuario)
                                                                .Where(x => x.Inicio.DayOfYear == dataAtual.DayOfYear)
                                                                .OrderBy(x => x.Inicio)
                                                                .ToList();
                
                var eventosNaoAlocados = eventosPendentes.Where(e => e.Status == true).ToList();

                List<(string, int)> temposDoDia = ObterTemposDoDia(eventosMarcados);


                foreach (var eventoPendente in eventosNaoAlocados)
                {
                    temposDoDia = AtualizarperiodoPermitido(temposDoDia, eventoPendente);
                    foreach (var tempo in temposDoDia)
                    {
                        if (eventoPendente.TempoEstimado <= tempo.Item2 && tempo.Item1 == "livre")
                        {
                            var index = temposDoDia.IndexOf(tempo);

                            int tempoEvento = eventoPendente.TempoEstimado;

                            int tempoRestante = (tempo.Item2 - tempoEvento) / 2;

                            temposDoDia.Insert(index, ("livre", tempoRestante));

                            temposDoDia[index + 1] = ("ocupado", tempoEvento);

                            temposDoDia.Insert(index + 2, ("livre", tempoRestante));

                            eventoPendente.Status = false;

                            break;

                        }
                    }
                }
                lists[i] = temposDoDia;
                todosEventosAlocados = eventosPendentes.All(e => e.Status == false);
            }
            Console.WriteLine(lists.ToString());
        }

        public List<(string, int)> ObterTemposDoDia(ICollection<EventosMarcados> eventosMarcados)
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

            if(periodoFimMinutos == 0)
                periodoFimMinutos = 1439;

            if (periodoInicioMinutos == 0)
                periodoInicioMinutos = 1;

            var minutos = 0;
            int i = -1;

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

            return temposDoDia;
        }
    }
}