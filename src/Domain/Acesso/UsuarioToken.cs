using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Acesso
{
    public class UsuarioToken
    {
        public UsuarioToken(DateTime validade)
        {
            this.Codigo = Guid.NewGuid();
            this.Validade = validade;
        }

        public Guid Codigo { get; private set; }
        public DateTime Validade { get; private set; }

        public bool EstaValido 
        {
            get 
            {
                return DateTime.Compare(DateTime.Now, Validade) < 0;
            }
        }

        public void Desativar()
        {
            this.Validade = DateTime.MinValue;
        }
    }
}
