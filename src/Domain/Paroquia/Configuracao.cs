using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Paroquia
{
    public class Configuracao
    {
        public DateTime AgendamentoInicio { get; set; }
        public DateTime AgendamentoFim { get; set; }
        public int IntervaloReserva { get; set; }
    }
}
