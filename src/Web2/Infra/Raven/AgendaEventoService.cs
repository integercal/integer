using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Domain.Agenda;
using Integer.Domain.Agenda.Exceptions;
using Integer.Infrastructure.Repository.Indexes;
using Raven.Client;

namespace Web.Infra.Raven
{
    public class AgendaEventoService
    {
        IDocumentSession session;

        public AgendaEventoService(IDocumentSession session)
        {
            this.session = session;
        }

        public void Agendar(Evento evento) 
        {
            // TODO: validar abertura conforme config da Paróquia
            VerificarOutrosEventos(evento);
            session.Store(evento);
        }

        private void VerificarOutrosEventos(Evento evento)
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
                    {
                        var ev = session.Load<Evento>(eventoAgendado.Id);
                        ev.AdicionarConflito(evento, MotivoConflitoEnum.ExisteEventoPrioritarioNaData);
                    }
                }
                else if (eventoAgendado.Evento.VerificarSeReservouLocais(evento.Reservas))
                {
                    if (tipoAgendado.NivelPrioridade < tipoEvento.NivelPrioridade
                        || (tipoAgendado.NivelPrioridade == tipoEvento.NivelPrioridade && eventoAgendado.Evento.DataCadastro < evento.DataCadastro))
                        eventosQueReservaramMesmoLocal.Add(eventoAgendado.Evento);
                    else
                    {
                        var ev = session.Load<Evento>(eventoAgendado.Id);
                        ev.AdicionarConflito(evento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
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