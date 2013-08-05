using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Acesso;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Integer.Infrastructure.Repository.Indexes
{
    public class Usuarios_PorToken : AbstractIndexCreationTask<Usuario, Usuarios_PorToken.Result>
    {
        public class Result 
        {
            public Guid Token { get; set; }
            public Usuario Usuario { get; set; }
        }

        public Usuarios_PorToken()
        {
            Map = usuarios => from usuario in usuarios 
                              from token in usuario.Tokens
                              select new {
                                  Token = token.Codigo,
                                  Usuario = usuario
                              };

            Store(x => x.Token, FieldStorage.Yes);
            Store(x => x.Usuario, FieldStorage.Yes);
        }
    }
}
