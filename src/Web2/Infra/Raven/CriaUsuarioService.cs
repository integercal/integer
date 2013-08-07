using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Domain.Acesso;
using Integer.Domain.Acesso.Exceptions;
using Integer.Infrastructure.Email.TemplateModels;
using Integer.Infrastructure.Tasks;
using Raven.Client;

namespace Web.Infra.Raven
{
    public class CriaUsuarioService
    {
        private IDocumentSession session;

        public CriaUsuarioService(IDocumentSession session)
        {
            this.session = session;
        }

        public Usuario CriarAdmin(string email, string nome) 
        {
            VerificaEmailExistente(email);

            var usuario = new Usuario(email, nome);
            session.Store(usuario);

            EnviaSenhaPorEmail(usuario);

            return usuario;
        }

        private void VerificaEmailExistente(string email)
        {
            var existingWithEmail = session.Query<Usuario>().SingleOrDefault(u => u.Email == email);
            if (existingWithEmail != null)
                throw new UsuarioExistenteException(existingWithEmail);
        }

        private void EnviaSenhaPorEmail(Usuario usuario) 
        {
            var model = new NovoUsuarioModel { 
                Nome = usuario.Nome, 
                Email = usuario.Email,
                Senha = usuario.Senha
            };
            TaskExecutor.ExcuteLater(new SendEmailTask("[Calendário Paroquial] - Usuário", "NovoUsuario", usuario.Email, model));
        }
    }
}