using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Exceptions;

namespace Integer.Domain.Acesso.Exceptions
{
    public class UsuarioExistenteException : IntegerException
    {
        private Usuario usuario;

        public UsuarioExistenteException(Usuario usuario) : base()
        {
            this.usuario = usuario;
        }

        public override string Message
        {
            get
            {
                return String.Format(AcessoResourceWrapper.ExistingUserWithEmail.Replace("#email#", usuario.Email));
            }
        }
    }
}
