using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integer.Domain.Agenda
{
    [Serializable]
    public class RSVP
    {
        public RSVP(string idUsuario)
        {
            IdUsuario = idUsuario;
        }

        public string IdUsuario { get; set; }
    }
}
