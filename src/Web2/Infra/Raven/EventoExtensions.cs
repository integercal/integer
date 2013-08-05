using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Agenda;
using Integer.Infrastructure.Repository.Indexes;
using Integer.Domain.Agenda.Exceptions;

namespace Web.Infra.Raven
{
    public static class EventoExtensions
    {
        public static IEnumerable<Evento_ParaCalendario.EventResult> ObterEventosAgendados(this IDocumentSession session, DateTime inicio, DateTime fim)
        {
            var eventos = new List<Evento_ParaCalendario.EventResult>();
            eventos = session.Query<Evento_ParaCalendario.EventResult, Evento_ParaCalendario>().Where(e => e.State == EstadoEventoEnum.Agendado
                                                        && ((inicio <= e.Start && e.Start <= fim)
                                                            || (inicio <= e.End && e.End <= fim))).ToList();
            return eventos;
        }

        public static IEnumerable<Evento> ObterEventos(this IDocumentSession session, DateTime inicio, DateTime fim)
        {
            var eventos = new List<Evento>();
            eventos = session.Query<Evento>().Where(e => (inicio <= e.DataInicio && e.DataInicio <= fim)
                                                                || (inicio <= e.DataFim && e.DataFim <= fim)).ToList();
            return eventos;
        }

        public static void Agendar(this IDocumentSession session, Evento novoEvento)
        {
            // TODO: validar abertura conforme config da Paróquia
            VerificarOutrosEventos(session, novoEvento);
            session.Store(novoEvento);
        }

        private static void VerificarOutrosEventos(IDocumentSession session, Evento evento)
        {
            DateTime inicio = evento.DataInicio;
            DateTime fim = evento.DataFim;
            TipoEvento tipoEvento = session.Load<TipoEvento>(evento.TipoEvento.Id);

            var eventosNaData = session.Query<Evento_ParaAgendamento.Result, Evento_ParaAgendamento>()
                .Where(e => e.Id != evento.Id && e.Estado == EstadoEventoEnum.Agendado
                         && ((inicio <= e.DataInicio && e.DataInicio <= fim) 
                         || (inicio <= e.DataFim && e.DataFim <= fim)
                         || (e.DataInicio <= inicio && fim <= e.DataFim))).ToList();

            var eventosPrioritarios = new List<Evento>();
            var eventosQueReservaramMesmoLocal = new List<Evento>();
            foreach (var eventoAgendado in eventosNaData)
            {
                var tipoAgendado = eventoAgendado.Tipo;
                if (tipoAgendado.IndependenteLocal)
                {                    
                    if (tipoAgendado.NivelPrioridade < tipoEvento.NivelPrioridade 
                        || (tipoAgendado.NivelPrioridade == tipoEvento.NivelPrioridade && eventoAgendado.Evento.DataCadastro < evento.DataCadastro))
                        eventosPrioritarios.Add(eventoAgendado.Evento);
                    else
                        eventoAgendado.Evento.AdicionarConflito(evento, MotivoConflitoEnum.ExisteEventoPrioritarioNaData);
                }
                else if (eventoAgendado.Evento.VerificarSeReservouLocais(evento.Reservas)) 
                {
                    if (tipoAgendado.NivelPrioridade < tipoEvento.NivelPrioridade
                        || (tipoAgendado.NivelPrioridade == tipoEvento.NivelPrioridade && eventoAgendado.Evento.DataCadastro < evento.DataCadastro))
                        eventosQueReservaramMesmoLocal.Add(eventoAgendado.Evento);
                    else
                    {
                        eventoAgendado.Evento.AdicionarConflito(evento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
                        session.Store(eventoAgendado.Evento);
                    }
                }
            }
            if (eventosPrioritarios.Count > 0)
                throw new EventoPrioritarioException(eventosPrioritarios);
            else if (eventosQueReservaramMesmoLocal.Count > 0)
                throw new LocalReservadoException(eventosQueReservaramMesmoLocal, evento);
        }
    }
}