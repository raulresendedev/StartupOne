using StartupOne.Dto.EventoMarcado;
using StartupOne.Dto.Interesses;
using StartupOne.Models;
using StartupOne.Repository;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace StartupOne.Service
{
    public class InteressesService
    {
        private readonly InteressesRepository _interessesRepository;
        private readonly EventoMarcadoRepository _eventosMarcadosRepository;

        private readonly TokenService _tokenService;
        public InteressesService(InteressesRepository interessesRepository, EventoMarcadoRepository eventosMarcadosRepository, TokenService tokenService)
        {
            _eventosMarcadosRepository = eventosMarcadosRepository;
            _tokenService = tokenService;
            _interessesRepository = interessesRepository;
        }

        DateTime dataAtual = DateTime.Now.Date;

        public void ValidarInteresse(Interesses interesse)
        {

            if (interesse.Prioridade == null)
                throw new Exception("Campo Prioridade é obrigatório");

            if (string.IsNullOrWhiteSpace(interesse.Nome))
                throw new Exception("Campo Nome é obrigatório");

            if (interesse.Prioridade > 2 || interesse.Prioridade < 0)
                throw new Exception("Campo Prioridade inválido");

            if (interesse.TempoEstimado.TimeOfDay.TotalMinutes < 5)
                throw new Exception("Campo de tempo estimado deve ter o mínimo de 5");

            if (interesse.TempoEstimado.TimeOfDay.TotalMinutes > 1440)
                throw new Exception("Campo de tempo estimado deve ter o máximo de 1440");

            if (interesse.PeriodoInicio == interesse.PeriodoFim)
                throw new Exception("Periodo fim e início não podem ser iguais.");

            if (interesse.PeriodoFim < interesse.PeriodoInicio)
                throw new Exception("Periodo fim não pode ser menor do que periodo início.");

            if ((interesse.PeriodoFim.TimeOfDay.TotalMinutes - interesse.PeriodoInicio.TimeOfDay.TotalMinutes) * 1 < interesse.TempoEstimado.TimeOfDay.TotalMinutes)
                throw new Exception("Tempo estimado não pode ser maior que o período");

            if (interesse.IdUsuario != _tokenService.GetUserIdFromToken())
                throw new Exception("Você não tem permissão para realizar esta operação.");

            if (interesse.Status != true) interesse.Status = true;

        }

        public InteressesDto CadastrarInteresse(InteressesDto interesseDto)
        {
            Interesses novoInteresse = new Interesses(
                idInteresse: 0,
                idUsuario: _tokenService.GetUserIdFromToken(),
                nome: interesseDto.Nome,
                categoria: interesseDto.Categoria,
                prioridade: interesseDto.Prioridade,
                status: interesseDto.Status,
                tempoEstimado: interesseDto.TempoEstimado,
                periodoInicio: interesseDto.PeriodoInicio,
                periodoFim: interesseDto.PeriodoFim
                
            );

            ValidarInteresse(novoInteresse);

            _interessesRepository.Adicionar(novoInteresse);

            return new InteressesDto
            {
                idInteresse = novoInteresse.IdInteresse,
                PeriodoInicio = novoInteresse.PeriodoInicio,
                PeriodoFim = novoInteresse.PeriodoFim,
                Nome = novoInteresse.Nome,
                TempoEstimado = novoInteresse.TempoEstimado,
                Status = novoInteresse.Status,
                Prioridade = novoInteresse.Prioridade
            };
        }

        public InteressesDto AtualizarInteresse(InteressesDto interesseDto)
        {
            Interesses interesseAtualizado = _interessesRepository.Obter(interesseDto.idInteresse);

            interesseAtualizado.Nome = interesseDto.Nome;
            interesseAtualizado.PeriodoInicio = interesseDto.PeriodoInicio;
            interesseAtualizado.PeriodoFim = interesseDto.PeriodoFim;
            interesseAtualizado.TempoEstimado = interesseDto.TempoEstimado;
            interesseAtualizado.Prioridade = interesseDto.Prioridade;

            ValidarInteresse(interesseAtualizado);

            _interessesRepository.Atualizar(interesseAtualizado);

            return interesseDto;
        }

        public void DeletarInteresse(int idInteresse)
        {
            Interesses interesse = _interessesRepository.Obter(idInteresse);

            if (interesse == null) throw new Exception("O interesse não foi encontrado");

            if (interesse.IdUsuario != _tokenService.GetUserIdFromToken()) throw new UnauthorizedAccessException("Você não tem permissão para excluir este interesse.");

            _interessesRepository.Remover(interesse);
        }

        public Interesses ObterEvento(int idEvento)
        {
            return _interessesRepository.Obter(idEvento);
        }

        public ICollection<Interesses> ObterInteressesDoUsuario(int idUsuario)
        {
            return _interessesRepository.ObterInteressesDoUsuario(idUsuario);
        }

        public string EncontrarHorarios(int idUsuario)
        {
            bool todosEventosAlocados = false;
            int i = 0;

            ICollection<EventoMarcado> eventosMarcadosNoDia = new List<EventoMarcado>();
            List<(string, int)> temposDoDia = new List<(string, int)>();
            ICollection<Interesses> eventosPendentes = _interessesRepository
                                                                .ObterInteressesDoUsuario(idUsuario)
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
                    eventosMarcadosNoDia = _eventosMarcadosRepository.ObterEventosPendentesDoUsuario(idUsuario)
                                                                .Where(x => x.Inicio.DayOfYear == dataAtual.DayOfYear)
                                                                .OrderBy(x => x.Inicio)
                                                                .ToList();

                    int intervalo = eventoPendente.TempoEstimado.TimeOfDay.TotalMinutes == 1440 ? 0 : 10;
                    
                    temposDoDia = ObterTemposDoDia(eventosMarcadosNoDia);
                    temposDoDia = AtualizarperiodoPermitido(temposDoDia, eventoPendente);
                    foreach (var tempo in temposDoDia)
                    {
                        if (eventoPendente.TempoEstimado.TimeOfDay.TotalMinutes + intervalo <= tempo.Item2 && tempo.Item1 == "livre")
                        {
                            AlocarEventoPendente(temposDoDia, tempo, eventoPendente);
                            break;
                        }
                    }
                }
                todosEventosAlocados = eventosPendentes.All(e => e.Status == false);
            }
            return "Ok";
        }
        public void AlocarEventoPendente(List<(string, int)> temposDoDia, (string, int) tempo, Interesses eventoPendente)
        {
            var index = temposDoDia.IndexOf(tempo);

            int tempoEvento = ((int)eventoPendente.TempoEstimado.TimeOfDay.TotalMinutes);

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

            _interessesRepository.Atualizar(eventoPendente);

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
        public List<(string, int)> AtualizarperiodoPermitido(List<(string, int)> temposDoDia, Interesses eventoPendente)
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