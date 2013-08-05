using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Integer.Domain.Paroquia;
using Web.Infra.AutoMapper;
using Web.Models;

namespace Web.Controllers
{
    public class GruposController : IntegerController
    {
        private readonly Grupos grupos;

        public GruposController(Grupos grupos)
        {
            this.grupos = grupos;
        }

        public HttpResponseMessage Get() 
        {
            var existingGroups = grupos.Todos().OrderBy(l => l.Nome).ToList(); 
            return Request.CreateResponse(HttpStatusCode.OK, existingGroups.MapTo<GroupModel>());
        }

        public HttpResponseMessage Post(GroupModel newGroup)
        {
            Grupo parent = null;
            if (!String.IsNullOrWhiteSpace(newGroup.ParentId))
                parent = RavenSession.Load<Grupo>(newGroup.ParentId);

            var group = new Grupo(newGroup.Name, newGroup.Email, newGroup.Color, parent);
            RavenSession.Store(group);

            return Request.CreateResponse(HttpStatusCode.OK, group);
        }

        public HttpResponseMessage Put(GroupModel editGroup)
        {
            var existingGroup = RavenSession.Load<Grupo>(editGroup.Id);
            if (existingGroup == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Grupo parent = GetGroup(editGroup.ParentId);

            existingGroup.Alterar(editGroup.Name, editGroup.Email, editGroup.Color, parent);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private Grupo GetGroup(string id) {
            Grupo group = null;
            if (!String.IsNullOrWhiteSpace(id))
                group = RavenSession.Load<Grupo>(id);

            return group;
        }

        public HttpResponseMessage Delete(string id)
        {
            var existingGroup = RavenSession.Load<Grupo>(id);
            RavenSession.Delete<Grupo>(existingGroup);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
