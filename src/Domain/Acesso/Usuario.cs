using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Acesso.Exceptions;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Acesso
{
    public class Usuario
    {
        protected Usuario() 
        {
            Tokens = new List<UsuarioToken>();
            Ativo = true;
            DataCriacao = DateTime.Now;
        }

        public Usuario(string Nome, string email, string senha, string grupoId) : this()
        {
            this.Nome = Nome;
            this.Email = email;
            this.Senha = Encryptor.Encrypt(senha);
            this.GrupoId = grupoId;
            PrecisaTrocarSenha = true;
            Role = "agente";
        }

        public Usuario(string email, string facebookId, string genero, string nome, string username) : this()
        {
            Email = email;
            FacebookId = facebookId;
            Genero = genero;
            Nome = nome;
            Username = username;
            Role = "paroquiano";
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool Ativo { get; private set; }
        public string GrupoId { get; set; }
        public bool PrecisaTrocarSenha { get; set; }
        public IList<UsuarioToken> Tokens { get; set; }
        public DateTime DataCriacao { get; private set; }
        public string Role { get; set; }

        public string FacebookId { get; set; }
        public string Genero { get; set; }
        public string Username { get; set; }

        public void TrocarSenha(string senha, Guid token)
        {
            UsuarioToken usuarioToken = Tokens.FirstOrDefault(t => t.Codigo == token);
            if (!usuarioToken.EstaValido)
                throw new UsuarioTokenExpiradoException();
            else
                usuarioToken.Desativar();

            this.Senha = Encryptor.Encrypt(senha);
        }

        public void CriarSenha(string senha) 
        {
            this.Senha = Encryptor.Encrypt(senha);
            PrecisaTrocarSenha = false;
        }

        public Guid CriarTokenParaTrocarSenha() 
        {
            var token = new UsuarioToken(DateTime.Now.AddMinutes(10));
            Tokens.Add(token);
            return token.Codigo;
        }

        public Guid CriarTokenParaLogin()
        {
            var tokenValido = Tokens.FirstOrDefault(t => t.EstaValido);
            if (tokenValido == null)
            {
                tokenValido = new UsuarioToken(DateTime.Now.AddYears(1));
                Tokens.Add(tokenValido);
            }
            return tokenValido.Codigo;
        }

        public void Alterar(string email, string facebookId, string gender, string name, string username)
        {
            Email = email;
            FacebookId = facebookId;
            Genero = gender;
            Nome = name;
            Username = username;
        }
    }
}
