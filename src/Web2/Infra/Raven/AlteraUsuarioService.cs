using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Domain.Acesso;
using Integer.Domain.Acesso.Exceptions;
using Raven.Client;

namespace Web.Infra.Raven
{
    public class AlteraUsuarioService
    {
        private IDocumentSession session;

        public AlteraUsuarioService(IDocumentSession session)
        {
            this.session = session;
        }

        public void Alterar(string id, string email, string nome) 
        {
            var existingUser = session.Load<Usuario>(id);
            if (existingUser == null)
                throw new Exception();

            VerificaEmailExistente(id, email);

            existingUser.Alterar(email, nome);
        }

        private void VerificaEmailExistente(string id, string email)
        {
            var existingWithEmail = session.Query<Usuario>().SingleOrDefault(u => u.Email == email);
            if (existingWithEmail != null && id != existingWithEmail.Id)
                throw new UsuarioExistenteException(existingWithEmail);
        }
    }
}