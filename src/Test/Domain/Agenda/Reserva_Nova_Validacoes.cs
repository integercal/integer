using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Reserva_Nova_Validacoes
    {
        Reserva reserva;
        Local local;

        public Reserva_Nova_Validacoes() 
        {
            local = MockRepository.GenerateStub<Local>();
        }

        private void Cria_Reserva() 
        {
            reserva = new Reserva(local);
        }

        [Fact]
        public void QuandoLocalEhNulo_DisparaExcecao()
        {
            local = null;
            Assert.Throws<DbCException>(() => Cria_Reserva());
        }
    }
}
