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
    public class UsuariosController : IntegerController
    {
        private readonly Usuarios usuarios;
        private readonly Grupos grupos;

        public UsuariosController(Usuarios usuarios, Grupos grupos)
        {
            this.usuarios = usuarios;
            this.grupos = grupos;
        }

        public HttpResponseMessage Get()
        {
            var existing = RavenSession.Query<Usuario>().ToList().OrderBy(l => l.Role);
            return Request.CreateResponse(HttpStatusCode.OK, existing.MapTo<UserModel>());
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(string email, string senha) 
        {
            Usuario usuario = RavenSession.ObterUsuario(email, senha);
            if (usuario == null)
                return Request.CreateResponse(HttpStatusCode.Forbidden, new { Error = AcessoResourceWrapper.InvalidLogin });

            return Request.CreateResponse(HttpStatusCode.OK, new { Token = usuario.CriarTokenParaLogin() });
        }

        public HttpResponseMessage Get(Guid token) 
        {
            Usuario usuario = usuarios.ComToken(token);
            if (usuario == null)
                Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, usuario.MapTo<CurrentUserModel>());
        }

        [AllowAnonymous]
        public HttpResponseMessage Put(FacebookUserModel facebookUserModel) 
        {
            Usuario usuario = RavenSession.ObterUsuario(facebookUserModel.FacebookId);
            if (usuario == null)
                usuario = RavenSession.CriarUsuario(facebookUserModel.Email, facebookUserModel.FacebookId, facebookUserModel.Gender, facebookUserModel.Name, facebookUserModel.Username);
            else
                usuario.Alterar(facebookUserModel.Email, facebookUserModel.FacebookId, facebookUserModel.Gender, facebookUserModel.Name, facebookUserModel.Username);

            return Request.CreateResponse(HttpStatusCode.OK, new { Token = usuario.CriarTokenParaLogin() });
        }

        public HttpResponseMessage Post(UserModel user) 
        {
            var newUser = RavenSession.CriarUsuario(user.Email, user.Name);
            return Request.CreateResponse(HttpStatusCode.OK, newUser.MapTo<UserModel>());
        }

        public HttpResponseMessage Put(UserModel user)
        {
            RavenSession.AlterarUsuario(user.Id, user.Email, user.Name);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
