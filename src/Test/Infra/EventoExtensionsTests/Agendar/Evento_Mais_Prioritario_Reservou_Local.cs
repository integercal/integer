using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Agenda;
using Integer.Domain.Agenda.Exceptions;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.DateAndTime;
using Rhino.Mocks;
using Web.Infra.Raven;
using Xunit;

namespace Integer.UnitTests.Infra.EventoExtensionsTests.Agendar
{
    public class Evento_Mais_Prioritario_Reservou_Local : InMemoryDataBaseTest
    {
        AgendaEventoService sut;

        Evento eventoExistente, eventoNovo;
        Local localDesejado;
        DateTime dataAtual;
        TipoEvento tipoMaisPrioritario, tipoMenosPrioritario;

        public Evento_Mais_Prioritario_Reservou_Local() 
        {
            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;

            Preparar();

            sut = new AgendaEventoService(DataBaseSession);
        }

        [Fact]
        public void QuandoAReservaNova_ConflitaComLocalDoEventoPrioritarioExistente_DisparaExcecao()
        {
            eventoNovo = CriarEvento(tipoMenosPrioritario, dataAtual, dataAtual.AddHours(4));
            eventoNovo.Reservar(localDesejado);

            Assert.Throws<LocalReservadoException>(() => sut.Agendar(eventoNovo));
        }

        private void Preparar()
        {
            CriarLocal();
            CriarTipos();
            CriarEventoExistenteQueReservouLocal(tipoMaisPrioritario);
        }

        private void CriarLocal()
        {
            localDesejado = new Local("Um Local");
            DataBaseSession.Store(localDesejado);
        }

        private void CriarTipos()
        {
            tipoMenosPrioritario = new TipoEvento("menos", 2);
            DataBaseSession.Store(tipoMenosPrioritario);

            tipoMaisPrioritario = new TipoEvento("mais", 1);
            DataBaseSession.Store(tipoMaisPrioritario);
        }

        private void CriarEventoExistenteQueReservouLocal(TipoEvento tipoEvento)
        {
            eventoExistente = CriarEvento(tipoEvento, dataAtual, dataAtual.AddHours(4));            
            eventoExistente.Reservar(localDesejado);

            DataBaseSession.Store(eventoExistente);
            DataBaseSession.SaveChanges();
        }

        private Evento CriarEvento(TipoEvento tipo, DateTime dataInicio, DateTime dataFim)
        {
            var grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }
    }
}
