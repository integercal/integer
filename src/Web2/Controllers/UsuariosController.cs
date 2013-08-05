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

        [AllowAnonymous]
        public HttpResponseMessage Post(string email, string senha) 
        {
            Usuario usuario = RavenSession.ObterUsuario(email, senha);
            if (usuario == null)
            {
                Grupo grupo = grupos.Com(g => g.Email == email);
                if (grupo != null && grupo.PrecisaCriarUsuario && grupo.ValidarSenha(senha))
                {
                    usuario = RavenSession.CriarUsuario(grupo.Nome, grupo.Email, grupo.Senha, grupo.Id);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new { Error = AcessoResourceWrapper.InvalidLogin });
                }
            }

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
    }
}
