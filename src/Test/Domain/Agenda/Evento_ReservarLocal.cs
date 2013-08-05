using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using DbC;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_ReservarLocal
    {
        [Fact]
        public void AoReservarComSucesso_ReservaEstahAssociadoAEvento() 
        {
            Local local = MockRepository.GenerateStub<Local>();

            DateTime dataInicioEvento = DateTime.Now;
            DateTime dataFimEvento = dataInicioEvento.AddHours(4);
            Evento evento = CriarEvento(dataInicioEvento, dataFimEvento);
            evento.Reservar(local);

            Assert.Equal(1, evento.Reservas.Count());
            Reserva reservaEsperada = new Reserva(local);
            Assert.Equal(reservaEsperada.Local, evento.Reservas.First().Local);
        }

        [Fact]
        public void QuandoJaExisteReservaDeUmLocalParaOutroHorarioParecido_DisparaExcecao() 
        {
            Local local = MockRepository.GenerateStub<Local>();

            DateTime dataInicioEvento = DateTime.Now;
            DateTime dataFimEvento = dataInicioEvento.AddHours(4);
            Evento evento = CriarEvento(dataInicioEvento, dataFimEvento);

            evento.Reservar(local);
            Assert.Throws<DbCException>(() => evento.Reservar(local));
        }

        private Evento CriarEvento(DateTime dataInicio, DateTime dataFim)
        {
            var grupo = MockRepository.GenerateStub<Grupo>();
            TipoEvento tipo = MockRepository.GenerateStub<TipoEvento>();
            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }
    }
}
