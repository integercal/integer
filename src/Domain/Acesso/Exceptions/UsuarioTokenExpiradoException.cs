using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Exceptions;

namespace Integer.Domain.Acesso.Exceptions
{
    public class UsuarioTokenExpiradoException : IntegerException
    {
        public override string Message
        {
            get
            {
                return "Chave expirada. Solicite a troca de senha outra vez.";
            }
        }
    }
}
