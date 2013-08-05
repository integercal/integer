using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using Integer.Domain.Agenda;
using Integer.Infrastructure.DateAndTime;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Reserva_Nova_Criada_Corretamente
    {
        Reserva reserva;
        Local local;
        DateTime data;

        public Reserva_Nova_Criada_Corretamente() 
        {
            local = new Local("Um Local");
            data = DateTime.Now.Date;

            reserva = new Reserva(local);
        }

        [Fact]
        public void Mapeia_Local() 
        {
            Assert.Equal(local.Id, reserva.Local.Id);
        }
    }
}
