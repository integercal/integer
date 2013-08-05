using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Integer.Infrastructure.Repository.Indexes
{
    public class Evento_ParaAgendamento : AbstractIndexCreationTask<Evento>
    {
        public class Result
        {
            public string Id { get; set; }
            public EstadoEventoEnum Estado { get; set; }
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }

            public Evento Evento { get; set; }
            public TipoEvento Tipo { get; set; }
        }

        public Evento_ParaAgendamento()
        {
            Map = eventos => from evento in eventos
                             select new
                             {
                                 Id = evento.Id,
                                 Estado = evento.Estado,
                                 DataInicio = evento.DataInicio,
                                 DataFim = evento.DataFim,
                                 Evento = evento
                             };

            TransformResults = (database, eventos) =>
                from evento in eventos
                let eventType = database.Load<TipoEvento>(evento.TipoEvento.Id)
                select new
                {
                    Id = evento.Id,
                    Estado = evento.Estado,
                    DataInicio = evento.DataInicio,
                    DataFim = evento.DataFim,
                    Evento = evento,
                    Tipo = eventType
                };

            Store(e => e.Id, FieldStorage.Yes);
            Store(e => e.Estado, FieldStorage.Yes);
            Store(e => e.DataInicio, FieldStorage.Yes);
            Store(e => e.DataFim, FieldStorage.Yes);
        }
    }
}
