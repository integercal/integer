using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Integer.Domain.Acesso;
using Integer.Domain.Paroquia;
using Web.Controllers;
using Web.Infra.Raven;
using Web.Infra.AutoMapper;
using Web2.Models;
using Integer.Infrastructure.I18n;
using Web.Models;
using Integer.Infrastructure.Criptografia;

namespace Web.Controllers
{
    public class ConfiguracoesController : IntegerController
    {
        public HttpResponseMessage Get() 
        {
            var config = RavenSession.Load<ParoquiaConfig>("paroquias-1"); // TODO: get from subdomain
            return Request.CreateResponse(HttpStatusCode.OK, config.MapTo<ConfigModel>());
        }
    }
}
