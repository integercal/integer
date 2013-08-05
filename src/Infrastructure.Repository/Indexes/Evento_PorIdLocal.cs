using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Agenda;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Integer.Infrastructure.Repository.Indexes
{
    public class Evento_PorIdLocal : AbstractIndexCreationTask<Evento>
    {
        public Evento_PorIdLocal()
        {
            Map = eventos => from evento in eventos  
                             from reserva in evento.Reservas
                             select new
                             {
                                 IdLocal = reserva.Local.Id,
                                 Reservas = evento.Reservas
                             };

            Store("IdLocal", FieldStorage.Yes);
        }
    }
}
