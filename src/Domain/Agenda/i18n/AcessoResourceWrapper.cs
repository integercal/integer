using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Agenda.i18n;
using Integer.Infrastructure.I18n;

namespace Integer.Domain.Agenda
{
    public class AgendaResourceWrapper
    {
        public static string EventReservedLocal 
        { 
            get 
            {
                return AgendaResource.EventReservedLocal;
            } 
        }

        public static string AlreadyRSVP
        {
            get
            {
                return AgendaResource.AlreadyRSVP;
            }
        }
    }
}
