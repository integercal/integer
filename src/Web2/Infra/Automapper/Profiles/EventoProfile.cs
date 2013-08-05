using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Domain.Agenda;
using System.Text;
using Web.Models;
using Integer.Infrastructure.Repository.Indexes;

namespace Web.Infra.AutoMapper.Profiles
{
    public class EventoProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Evento, EventModel>()
                .ForMember(x => x.Subject, o => o.MapFrom(m => m.Nome))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Descricao))
                .ForMember(x => x.Start, o => o.MapFrom(m => m.DataInicio.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(x => x.End, o => o.MapFrom(m => m.DataFim.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(x => x.Group, o => o.MapFrom(m => m.Grupo))
                .ForMember(x => x.Locals, o => o.MapFrom(m => m.Reservas))
                .ForMember(x => x.EventType, o => o.MapFrom(m => m.TipoEvento));            

            Mapper.CreateMap<Reserva, LocalModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Local.Id))
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Local.Nome));

            Mapper.CreateMap<RSVP, RSVPModel>();

            Mapper.CreateMap<TipoEvento, EventTypeModel>()
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Nome))
                .ForMember(x => x.Priority, o => o.MapFrom(m => m.NivelPrioridade));
        }
    }
}