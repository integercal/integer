using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Web.Infra.Raven;
using Xunit;

namespace Integer.UnitTests.Infra.EventoExtensionsTests.Agendar
{
    public class Evento_Menos_Prioritario_Reservou_Local : IUseFixture<Evento_Menos_Prioritario_Reservou_Local_Fixture>
    {
        Evento_Menos_Prioritario_Reservou_Local_Fixture fixture;

        public void SetFixture(Evento_Menos_Prioritario_Reservou_Local_Fixture data)
        {
            fixture = data;
        }

        [Fact]
        public void Salva_Evento_Novo_Com_Sucesso()
        {
            Assert.False(String.IsNullOrWhiteSpace(fixture.EventoNovo.Id));
        }

        [Fact]
        public void Evento_Existente_Fica_Com_Estado_Nao_Agendado()
        {
            Assert.Equal(EstadoEventoEnum.NaoAgendado, fixture.EventoExistente.Estado);
        }

        [Fact]
        public void Evento_Existente_Fica_Com_Conflito_Referente_Ao_Evento_Novo()
        {
            Assert.Equal(1, fixture.EventoExistente.Conflitos.Count());
            var conflito = fixture.EventoExistente.Conflitos.First();
            Assert.Equal(fixture.EventoNovo.Id, conflito.Evento.Id);
            Assert.Equal(MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade, conflito.Motivo);
        }
    }
}
