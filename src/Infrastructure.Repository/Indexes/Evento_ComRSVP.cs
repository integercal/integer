using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Integer.Domain.Agenda;
using Raven.Abstractions.Indexing;
using Integer.Domain.Acesso;

namespace Integer.Infrastructure.Repository.Indexes
{
    public class Evento_ComRSVP : AbstractIndexCreationTask<Evento>
    {
        public class EventoComRSVPResult
		{
            public string Id { get; set; }
            public string Subject { get; set; }
            public string Description { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public ReferenceResult Group { get; set; }
            public ReferenceResult EventType { get; set; }
            public IList<ReferenceResult> Locals { get; set; }
            public IList<RSVPResult> Rsvp { get; set; }
        }

        public class RSVPResult
        {
            public string IdUsuario { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
        }

        public class ReferenceResult 
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public Evento_ComRSVP()
        {
            Map = eventos => from evento in eventos
                             where evento.Estado != EstadoEventoEnum.Cancelado
                             select new 
                             {
                                 Id = evento.Id,
                                 Subject = evento.Nome,
                                 Description = evento.Descricao,
                                 Start = evento.DataInicio,
                                 End = evento.DataFim,
                                 Group = new ReferenceResult{ Id = evento.Grupo.Id, Name = evento.Grupo.Nome },
                                 EventType = new ReferenceResult { Id = evento.TipoEvento.Id, Name = evento.TipoEvento.Nome },
                                 Locals = evento.Reservas.Select(r => new { Id = r.Local.Id, Name = r.Local.Nome }),
                                 Rsvp = evento.Rsvp
                             };

            TransformResults = (database, eventos) =>
                from evento in eventos
                let rsvp = (from rsvp in evento.Rsvp
                            let usuario = database.Load<Usuario>(rsvp.IdUsuario)
                            select new RSVPResult { 
                                IdUsuario = usuario.Id,
                                Name = usuario.Nome,
                                Username = usuario.Username
                            })
                select new
                {
                    Id = evento.Id,
                    Subject = evento.Nome,
                    Description = evento.Descricao,
                    Start = evento.DataInicio,
                    End = evento.DataFim,
                    Group = new ReferenceResult{ Id = evento.Grupo.Id, Name = evento.Grupo.Nome },
                    EventType = new ReferenceResult { Id = evento.TipoEvento.Id, Name = evento.TipoEvento.Nome },
                    Locals = evento.Reservas.Select(r => new { Id = r.Local.Id, Name = r.Local.Nome }),
                    Rsvp = rsvp
                };

            Store(x => x.Id, FieldStorage.Yes);
        }
    }
}
