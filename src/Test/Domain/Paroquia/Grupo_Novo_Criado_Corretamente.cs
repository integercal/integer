using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Paroquia
{
    public class Grupo_Novo_Criado_Corretamente
    {
        Grupo grupo, grupoPai;

        string nome, email, cor;

        public Grupo_Novo_Criado_Corretamente()
        {
            nome = "Grupo";
            email = "grupo@Paroquia.com.br";
            grupoPai = MockRepository.GenerateStub<Grupo>();
            cor = "FFFFFF";

            CriarGrupo();
        }

        [Fact]
        public void Mapeia_Nome() 
        {
            Assert.Equal("Grupo", grupo.Nome);
        }

        [Fact]
        public void Mapeia_Email()
        {
            Assert.Equal("grupo@Paroquia.com.br", grupo.Email);
        }

        [Fact]
        public void Nao_Tem_Cor_Quando_Eh_Evento_Filho()
        {
            Assert.Null(grupo.Cor);
        }

        [Fact]
        public void Tem_Cor_Quando_Eh_Evento_Pai()
        {
            grupoPai = null;
            CriarGrupo();

            Assert.Equal("FFFFFF", grupo.Cor);
        }

        [Fact]
        public void Mapeia_GrupoPai() 
        {
            Assert.Equal(grupoPai.Id, grupo.GrupoPai.Id);
        }

        [Fact]
        public void PrecisaTrocarSenha() 
        {
            Assert.True(grupo.PrecisaCriarUsuario);
        }

        [Fact]
        public void Senha_EhPadrao_calendario2013() 
        {
            Assert.Equal("calendario2013", grupo.SenhaDescriptografada);
        }

        public void CriarGrupo() 
        {
            grupo = new Grupo(nome, email, cor, grupoPai);
        }
    }
}
