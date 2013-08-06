using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Models;
using Web.Infra.Raven;
using Integer.Domain.Agenda;
using AutoMapper;
using Web.Infra.AutoMapper;
using Web.Infra;
using System.Web.Http.ModelBinding;
using Integer.Domain.Paroquia;
using Integer.Domain.Agenda.Exceptions;
using Integer.Infrastructure.Exceptions;
using Integer.Infrastructure.Repository.Indexes;

namespace Web.Controllers
{
    public class EventosController : IntegerController
    {
        private readonly Grupos grupos;

        public EventosController(Grupos grupos)
        {
            this.grupos = grupos;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(string id)
        {
            var evento = RavenSession.Query<Evento_ComRSVP.EventoComRSVPResult, Evento_ComRSVP>().SingleOrDefault(e => e.Id == id);
            if (evento == null)
                Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, evento);
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(DateTime date, string viewType) 
        {
            DateTime start = date.GetIntervalStart(viewType);
            DateTime end = date.GetIntervalEnd(viewType);

            IEnumerable<Evento_ParaCalendario.EventResult> eventos = RavenSession.ObterEventosAgendados(start, end);

            return Request.CreateResponse(HttpStatusCode.OK, eventos.OrderBy(e => e.Start));
        }

        public HttpResponseMessage Post(EventModel newEvent)
        {
            var evento = CreateEvent(newEvent);
            RavenSession.Agendar(evento);
            newEvent.Id = evento.Id;

            return Request.CreateResponse(HttpStatusCode.OK, newEvent);
        }

        public HttpResponseMessage Put(EventModel editEvent)
        {
            var existingEvent = RavenSession.Load<Evento>(editEvent.Id);

            MapEvent(existingEvent, editEvent);
            RavenSession.Agendar(existingEvent);

            return Request.CreateResponse(HttpStatusCode.OK, existingEvent);
        }

        private Evento CreateEvent(EventModel input)
        {
            var grupo = RavenSession.Load<Grupo>(input.Group.Id);
            var tipo = RavenSession.Load<TipoEvento>(input.EventType.Id);

            Evento evento = new Evento(input.Subject, input.Description, input.Start, input.End, grupo, tipo);
            foreach (var local in input.Locals)
            {
                var exitingLocal = RavenSession.Load<Local>(local.Id);
                evento.Reservar(exitingLocal);
            }
            return evento;
        }

        private void MapEvent(Evento evento, EventModel input)
        {
            var grupo = RavenSession.Load<Grupo>(input.Group.Id);
            var tipo = RavenSession.Load<TipoEvento>(input.EventType.Id);

            evento.Alterar(input.Subject, input.Description, input.Start, input.End, grupo, tipo);
            
            var newLocals = new List<Local>();
            foreach (var local in input.Locals)
            {
                var existingLocal = RavenSession.Load<Local>(local.Id);
                newLocals.Add(existingLocal);
            }
            evento.AlterarLocais(newLocals);
        }

        [HttpPost]
        public HttpResponseMessage RSVP(string parentId, RSVPModel rsvp) 
        {
            var existingEvent = RavenSession.Load<Evento>(parentId);
            if (existingEvent != null)
                existingEvent.ConfirmarPresenca(rsvp.IdUser);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        [ActionName("RSVP")]
        public HttpResponseMessage RSVPDelete(string parentId, string idUser)
        {
            var existingEvent = RavenSession.Load<Evento>(parentId);
            if (existingEvent != null)
                existingEvent.RemoverPresenca(idUser);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
