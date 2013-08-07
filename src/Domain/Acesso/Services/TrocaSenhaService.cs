using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Acesso.Exceptions;
using Integer.Infrastructure.Email.TemplateModels;
using Integer.Infrastructure.Tasks;

namespace Integer.Domain.Acesso
{
    public class TrocaSenhaService
    {
        private Usuarios usuarios;

        public TrocaSenhaService(Usuarios usuarios)
        {
            this.usuarios = usuarios;
        }

        public void EnviarSenha(string email) 
        {
            var usuario = usuarios.Com(u => u.Email == email);
            if (usuario == null)
                throw new UsuarioInexistenteException();

            var token = usuario.CriarTokenParaTrocarSenha();

            TaskExecutor.ExcuteLater(new SendEmailTask("Trocar senha", "TrocarSenha", usuario.Email, new TrocarSenhaModel { UserId = usuario.Id, Token = token.ToString() }));
        }

        public void TrocarSenha(Guid token, string usuarioId, string senha)
        {
            var usuario = usuarios.ComId(usuarioId);
            if (usuario == null)
                throw new UsuarioInexistenteException();

            usuario.TrocarSenha(senha, token);
        }

        public void DesativarToken(string usuarioId, Guid codigoToken)
        {
            Usuario usuario = usuarios.ComId(usuarioId);
            var token = usuario.Tokens.FirstOrDefault(t => t.Codigo == codigoToken);

            token.Desativar();
        }
    }
}
