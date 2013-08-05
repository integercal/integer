using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Repository.Indexes;
using Web.Infra.AutoMapper;
using Web.Models;

namespace Web.Controllers
{
    public class TiposDeEventoController : IntegerController
    {
        public HttpResponseMessage Get()
        {
            var existing = RavenSession.Query<TipoEvento>().OrderBy(t => t.NivelPrioridade).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, existing.MapTo<EventTypeModel>());
        }

        public HttpResponseMessage Post(EventTypeModel newType)
        {
            var nextPriority = RavenSession.Query<TipoEvento_NextPriority.Result, TipoEvento_NextPriority>().SingleOrDefault();

            var type = new TipoEvento(newType.Name, nextPriority.Number+1);
            RavenSession.Store(type);

            return Request.CreateResponse(HttpStatusCode.OK, type.MapTo<EventTypeModel>());
        }

        public HttpResponseMessage Put(IEnumerable<EventTypeModel> types)
        {
            foreach (var type in types)
            {
                Update(type);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void Update(EventTypeModel editType)
        {
            var existing = RavenSession.Load<TipoEvento>(editType.Id);
            if (existing == null)
                throw new Exception();

            existing.Nome = editType.Name;
            existing.NivelPrioridade = editType.Priority;
        }

        public HttpResponseMessage Delete(string id)
        {
            var existing = RavenSession.Load<TipoEvento>(id);
            RavenSession.Delete<TipoEvento>(existing);
            UpdatePrioritiesWithout(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void UpdatePrioritiesWithout(string exceptId) 
        {
            var existing = RavenSession.Query<TipoEvento>().Where(t => t.Id != exceptId).OrderBy(x => x.NivelPrioridade).ToList();
            for (int i = 0; i < existing.Count; i++)
            {
                existing[i].NivelPrioridade = i + 1;
            }
        }
    }
}
