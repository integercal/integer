using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Enums;
using Integer.Infrastructure.Exceptions;

namespace Integer.Domain.Agenda.Exceptions
{
    public class LocalReservadoException : IntegerException
    {
        private readonly IEnumerable<Evento> eventos;
        private readonly Evento eventoReferencia;

        public LocalReservadoException(IEnumerable<Evento> eventos, Evento evento)
        {
            this.eventos = eventos;
            this.eventoReferencia = evento;
        }

        public override string Message
        {
            get
            {
                StringBuilder msgErro = new StringBuilder();
                foreach (Evento eventoPrioritario in eventos)
                {
                    msgErro.Append(AgendaResourceWrapper.EventReservedLocal.Replace("#eventname#", eventoPrioritario.Nome));
                    
                    StringBuilder msgReservas = new StringBuilder();
                    foreach (var reserva in eventoPrioritario.Reservas)
                    {
                        if (eventoReferencia.Reservas.Any(r => r.Local.Id == reserva.Local.Id)) {
                            var separator = msgReservas.Length == 0 ? " " : ", ";
                            msgReservas.Append(String.Format("{0}'{1}' ", separator, reserva.Local.Nome));
                        }
                    }
                    msgErro.AppendLine(msgReservas.ToString());
                }
                return msgErro.ToString();
            }
        }
    }
}
