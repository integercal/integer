﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Domain.Acesso;
using Integer.Infrastructure.Criptografia;
using Raven.Client;

namespace Web.Infra.Raven
{
    public static class UsuarioDocumentSessionExtensions
    {
        public static Usuario ObterUsuario(this IDocumentSession session, string email, string senha)
        {
            return session.Query<Usuario>().FirstOrDefault(u => u.Email == email && u.Senha == Encryptor.Encrypt(senha) && u.Ativo);
        }

        public static Usuario ObterUsuario(this IDocumentSession session, string facebookId)
        {
            return session.Query<Usuario>().FirstOrDefault(u => u.FacebookId == facebookId && u.Ativo);
        }

        public static Usuario CriarUsuario(this IDocumentSession session, string email, string facebookId, string gender, string name, string username)
        {
            var usuario = new Usuario(email, facebookId, gender, name, username);
            session.Store(usuario);

            return usuario;
        }

        public static Usuario CriarUsuario(this IDocumentSession session, string email, string name)
        {
            var criaUsuario = new CriaUsuarioService(session);
            var usuario = criaUsuario.CriarAdmin(email, name);
            //TODO: send email
            return usuario;
        }

        public static void AlterarUsuario(this IDocumentSession session, string id, string email, string nome) 
        {
            var alteraUsuario = new AlteraUsuarioService(session);
            alteraUsuario.Alterar(id, email, nome);
        }
    }
}