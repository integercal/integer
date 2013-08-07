using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Validation;
using System.Diagnostics.Contracts;
using DbC;
using Integer.Infrastructure.DocumentModelling;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Paroquia
{
    [Serializable]
    public class Grupo : INamedDocument
    {
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME = 50;

        public virtual string Id { get; set; }
        public virtual string Nome { get; set; }
        public string Email { get; private set; }
        public DenormalizedReference<Grupo> GrupoPai { get; private set; }
        public string ParoquiaId { get; set; }
        public string Cor { get; set; }

        protected Grupo() 
        {
        }

        public Grupo(string nome, string email, string cor, Grupo grupoPai)
        {
            PreencherNome(nome);
            PreencherGrupoPai(grupoPai);

            // TODO validar email
            this.Email = email;

            this.Cor = (grupoPai == null) ? cor.Replace("#", "") : null;
        }

        public void PreencherNome(string nome)
        {
            #region pré-condição
            var nomeFoiInformado = Assertion.That(!String.IsNullOrWhiteSpace(nome))
                                            .WhenNot("Necessário informar o nome do grupo.");
            var nomePossuiQuantidadeDeCaracteresValida = Assertion.That(nome != null && nome.Trim().Length <= NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME)
                                                                  .WhenNot(String.Format("O nome do grupo não pode ultrapassar o tamanho de {0} caracteres.", 
                                                                                         NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME.ToString()));
            #endregion
            (nomeFoiInformado & nomePossuiQuantidadeDeCaracteresValida).Validate();
            
            Nome = nome.Trim();
        }

        private void PreencherGrupoPai(Grupo grupo)
        {
            this.GrupoPai = grupo;
        }

        public void Alterar(string nome, string email, string cor, Grupo grupoPai)
        {
            PreencherNome(nome);
            PreencherGrupoPai(grupoPai);
            this.Email = email;
            this.Cor = (grupoPai == null) ? cor.Replace("#", "") : null;
            
        }
    }
}
