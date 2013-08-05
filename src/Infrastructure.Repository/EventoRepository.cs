using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Raven.Client;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;

namespace Integer.Infrastructure.Repository
{
    public class EventoRepository : Eventos
    {
        private IDocumentSession documentSession;

        public EventoRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public IEnumerable<Evento> Com(IEnumerable<string> ids) 
        {
            return documentSession.Load<Evento>(ids);
        }

        public IEnumerable<Evento> Todos(Expression<Func<Evento, bool>> condicao)
        {
            var ev = documentSession.Query<Evento>()
                .Where(e => e.Estado != EstadoEventoEnum.Cancelado)
                .Where(condicao).ToList();

            return ev;
        }

        public void Salvar(Evento evento)
        {
            documentSession.Store(evento);
        }

        public IEnumerable<Evento> QuePossuemConflitosCom(Evento evento)
        {
            return documentSession.Query<Evento>().Where(e => e.Conflitos.Any(c => c.Evento.Id.Equals(evento.Id)));
        }

        public IEnumerable<Evento> QuePossuemConflitoCom(Evento evento, MotivoConflitoEnum motivo)
        {
            return documentSession.Query<Evento>().Where(e => e.Conflitos.Any(c => c.Evento.Id.Equals(evento.Id) && c.Motivo == motivo));
        }

        public IEnumerable<Evento> QueReservaramOMesmoLocal(Evento evento)
        {
            DateTime inicioComIntervaloMinimo = evento.DataInicio.Subtract(Horario.INTERVALO_MINIMO_ENTRE_EVENTOS_E_RESERVAS);
            DateTime fimComIntervaloMinimo = evento.DataFim.Add(Horario.INTERVALO_MINIMO_ENTRE_EVENTOS_E_RESERVAS);

            var reservas = evento.Reservas.ToList();
            if (reservas.Count == 0)
            {
                return new List<Evento>();
            }
            else
            {
                var query = documentSession.Advanced.LuceneQuery<Evento>("ReservasMap");
                query = query.WhereLessThanOrEqual("DataInicio", inicioComIntervaloMinimo).AndAlso().WhereGreaterThanOrEqual("DataFim", inicioComIntervaloMinimo)
                    .OrElse().WhereLessThanOrEqual("DataInicio", fimComIntervaloMinimo).AndAlso().WhereGreaterThanOrEqual("DataFim", fimComIntervaloMinimo)
                    .OrElse().WhereGreaterThanOrEqual("DataInicio", inicioComIntervaloMinimo).AndAlso().WhereLessThanOrEqual("DataFim", fimComIntervaloMinimo);

                query = query.AndAlso();
                for (int i = 0; i < reservas.Count; i++)
                {
                    var reserva = reservas[i];
                    query = query
                        .Not.WhereEquals("Id", evento.Id)
                        .WhereEquals("IdLocal", reserva.Local.Id);

                    if (i + 1 < reservas.Count)
                        query = query.OrElse();
                }
                return query.ToList();
            }
        }
    }
}
