using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using DbC;
using Integer.Infrastructure.Events;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_AlterarReservasDeLocais
    {
        Local local;

        [Fact]
        public void QuandoAlteraLocal_EventoFicaComNovoLocal() 
        {
            Evento evento = CriarEventoComReserva();

            var outroLocal = new Local("Outro local");
            outroLocal.Id = "1";
            evento.AlterarLocais(new List<Local> { outroLocal });

            Assert.Equal(1, evento.Reservas.Count());
            var reservaEsperada = new Reserva(local);
            Assert.Equal(reservaEsperada.Local.Id, evento.Reservas.First().Local.Id);
        }

        private Evento CriarEventoComReserva()
        {
            var dataInicioEvento = DateTime.Now;
            var dataFimEvento = dataInicioEvento.AddHours(4);
            var grupo = MockRepository.GenerateStub<Grupo>();
            var evento = new Evento("Nome", "Descricao", dataInicioEvento, dataFimEvento, grupo, TipoEventoEnum.Comum);

            local = new Local("Local");
            local.Id = "1";

            evento.Reservar(local);

            return evento;
        }
    }
}
