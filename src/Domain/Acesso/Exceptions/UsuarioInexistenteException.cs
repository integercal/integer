using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Exceptions;

namespace Integer.Domain.Acesso.Exceptions
{
    public class UsuarioInexistenteException : IntegerException
    {
        public override string Message
        {
            get
            {
                return "Usuário não encontrado";
            }
        }
    }
}
