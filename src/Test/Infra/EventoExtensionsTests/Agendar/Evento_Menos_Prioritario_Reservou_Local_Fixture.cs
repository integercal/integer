using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.DateAndTime;
using Rhino.Mocks;
using Web.Infra.Raven;

namespace Integer.UnitTests.Infra.EventoExtensionsTests.Agendar
{
    public class Evento_Menos_Prioritario_Reservou_Local_Fixture : InMemoryDataBaseTest
    {
        AgendaEventoService sut;

        public Evento EventoExistente { get; set; }
        public Evento EventoNovo { get; set; }

        TimeSpan intervaloMinimoEntreReservas = TimeSpan.FromMinutes(59);
        Local localDesejado;
        DateTime dataReservaExistente;
        DateTime dataAtual;
        IEnumerable<TipoEvento> tipos;

        public Evento_Menos_Prioritario_Reservou_Local_Fixture()
        {
            sut = new AgendaEventoService(DataBaseSession);

            CriarNovoBancoDeDados();

            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;

            CriarTiposDeEvento();
            CriaReservaQueSobrepoe();
        }

        private void CriarTiposDeEvento()
        {
            tipos = new List<TipoEvento> { 
                new TipoEvento("primeiro", 1),
                new TipoEvento("segundo", 2),
            };
            foreach (var tipo in tipos)
            {
                DataBaseSession.Store(tipo);
            }
        }

        public void CriaReservaQueSobrepoe()
        {                   
            CriarEventoExistenteQueReservouLocal(tipos.ElementAt(1));

            EventoNovo = CriarEvento(tipos.ElementAt(0), dataAtual, dataAtual.AddHours(4));
            EventoNovo.Reservar(localDesejado);

            sut.Agendar(EventoNovo);
            DataBaseSession.SaveChanges();
        }

        private void CriarEventoExistenteQueReservouLocal(TipoEvento tipoEvento)
        {
            EventoExistente = CriarEvento(tipoEvento, dataAtual, dataAtual.AddHours(4));

            localDesejado = new Local("Um Local");
            localDesejado.Id = "1";

            EventoExistente.Reservar(localDesejado);

            DataBaseSession.Store(EventoExistente);
            DataBaseSession.SaveChanges();
        }

        private Evento CriarEvento(TipoEvento tipo, DateTime dataInicio, DateTime dataFim)
        {
            var grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }
    }
}
