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

namespace Web.Controllers
{
    public class UsuariosSenhaController : IntegerController
    {
        private readonly Usuarios usuarios;
        private readonly TrocaSenhaService trocaSenhaService;

        public UsuariosSenhaController(Usuarios usuarios, TrocaSenhaService trocaSenhaService)
        {
            this.usuarios = usuarios;
            this.trocaSenhaService = trocaSenhaService;
        }

        [AllowAnonymous]
        public HttpResponseMessage Post(string email) 
        {
            trocaSenhaService.EnviarSenha(email);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
