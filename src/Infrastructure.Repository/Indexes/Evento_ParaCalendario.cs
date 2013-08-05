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
    public class Evento_ParaCalendario : AbstractIndexCreationTask<Evento>
    {
        public class EventResult
        {
            public string Id { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string Subject { get; set; }
            public string Color { get; set; }
            public EstadoEventoEnum State { get; set; }
            public GroupResult Group { get; set; }
        }

        public class GroupResult 
        {
            public string Id { get; set; }
            public string Color { get; set; }
        }

        public Evento_ParaCalendario()
        {
            Map = eventos => from evento in eventos
                             select new
                             {
                                 Id = evento.Id,
                                 Subject = evento.Nome,
                                 Start = evento.DataInicio,
                                 End = evento.DataFim,
                                 State = evento.Estado,
                                 Group = new GroupResult { Id = evento.Grupo.Id }
                             };

            TransformResults = (database, eventos) =>
                from evento in eventos
                let groupEvent = database.Load<Grupo>(evento.Grupo.Id)
                let groupParent = database.Load<Grupo>(groupEvent.GrupoPai.Id)
                select new {
                    Id = evento.Id,
                    Subject = evento.Nome,
                    Start = evento.DataInicio,
                    End = evento.DataFim,
                    State = evento.Estado,
                    Color = groupEvent.Cor ?? groupParent.Cor
                };

            Store("Start", FieldStorage.Yes);
            Store("End", FieldStorage.Yes);
            Store("State", FieldStorage.Yes);
        }
    }
}
