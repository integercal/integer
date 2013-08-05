using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Integer.Domain.Agenda;
using Raven.Abstractions.Indexing;

namespace Integer.Infrastructure.Repository.Indexes
{
    public class ReservasMap : AbstractIndexCreationTask<Evento, ReservasMap.ReservasResult>
    {
        public class ReservasResult
		{
            public string Id { get; set; }
            public string IdLocal { get; set; }
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
        }

        public ReservasMap()
        {
            Map = eventos => from evento in eventos
                             from reserva in evento.Reservas
                             where evento.Estado != EstadoEventoEnum.Cancelado
                             select new 
                             {
                                 Id = evento.Id,
                                 DataInicio = evento.DataInicio,
                                 DataFim = evento.DataFim,
                                 IdLocal = reserva.Local.Id
                             };

            Store(x => x.Id, FieldStorage.Yes);
            Store(x => x.DataInicio, FieldStorage.Yes);
            Store(x => x.DataFim, FieldStorage.Yes);
            Store(x => x.IdLocal, FieldStorage.Yes);
        }
    }
}
