﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Raven.Client;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;
using Integer.Domain.Acesso;
using Integer.Infrastructure.Repository.Indexes;

namespace Integer.Infrastructure.Repository
{
    public class UsuarioRepository : Usuarios
    {
        private IDocumentSession documentSession;

        public UsuarioRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public Usuario ComId(string id) 
        {
            return documentSession.Load<Usuario>(id);
        }

        public Usuario Com(Expression<Func<Usuario, bool>> condicao)
        {
            return documentSession.Query<Usuario>().FirstOrDefault(condicao);
        }
        
        public void Salvar(Usuario usuario)
        {
            documentSession.Store(usuario);
        }

        public Usuario ComToken(Guid codigo)
        {
            Usuario usuario = null;

            var result = documentSession.Query<Usuario, Usuarios_PorToken>()
                .AsProjection<Usuarios_PorToken.Result>()
                .Where(x => x.Token == codigo).SingleOrDefault();

            if (result != null)
                usuario = result.Usuario;
            
            return usuario;
        }
    }
}
