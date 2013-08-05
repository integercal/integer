using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Exceptions;

namespace Integer.Domain.Agenda.Exceptions
{
    public class EventoPrioritarioException : IntegerException
    {
        private readonly IEnumerable<Evento> eventosPrioritarios;

        public EventoPrioritarioException(IEnumerable<Evento> eventosPrioritarios)
        {
            this.eventosPrioritarios = eventosPrioritarios;
        }

        public override string Message
        {
            get
            {
                StringBuilder msgErro = new StringBuilder();
                foreach (Evento evento in eventosPrioritarios)
                {                    
                    msgErro.AppendLine(String.Format("O evento paroquial '{0}' já está cadastrado para o horário: {1}.", evento.Nome, evento.Horario));
                }
                return msgErro.ToString();
            }
        }
    }
}
