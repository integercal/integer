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

        public static void Agendar(this IDocumentSession session, Evento evento)
        {
            var agenda = new AgendaEventoService(session);
            agenda.Agendar(evento);
        }
    }
}