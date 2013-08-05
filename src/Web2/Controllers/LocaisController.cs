using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DbC;
using Integer.Domain.Paroquia;
using Raven.Abstractions.Data;
using Web.Infra.AutoMapper;
using Web.Models;

namespace Web.Controllers
{
    public class LocaisController : IntegerController
    {
        private readonly Locais locais;

        public LocaisController(Locais locais)
        {
            this.locais = locais;
        }

        public HttpResponseMessage Get() 
        {
            var existingLocals = locais.Todos().OrderBy(l => l.Nome); 
            return Request.CreateResponse(HttpStatusCode.OK, existingLocals.MapTo<LocalModel>());
        }

        public HttpResponseMessage Post(LocalModel newLocal) 
        {
            var local = new Local(newLocal.Name);
            RavenSession.Store(local);

            return Request.CreateResponse(HttpStatusCode.OK, local);
        }

        public HttpResponseMessage Put(LocalModel editLocal)
        {
            var existingLocal = RavenSession.Load<Local>(editLocal.Id);
            if (existingLocal == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            existingLocal.Nome = editLocal.Name;
            //AtualizarReferencias(existingLocal);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void AtualizarReferencias(Local existingLocal)
        {
            RavenSession.Advanced.DocumentStore.DatabaseCommands.UpdateByIndex("Evento/PorIdLocal",
                new IndexQuery { Query = "IdLocal:" + existingLocal.Id},
                new ScriptedPatchRequest
                {
                    Script = @"
                    var local = LoadDocument('" + existingLocal.Id + @"');    
                    this.Reservas.Map(function(i) {
						if(i.Name=='Local')
							i.Value= { Id: local.Id, Nome: local.Nome };
						return i;
                    });
                    "
                });
        }

        public HttpResponseMessage Delete(string id) 
        {
            var existingLocal = RavenSession.Load<Local>(id);
            RavenSession.Delete<Local>(existingLocal);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
