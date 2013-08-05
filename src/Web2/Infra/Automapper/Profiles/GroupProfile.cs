using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using System.Text;
using Web.Models;
using Integer.Domain.Paroquia;
using Web2.Models;
using Integer.Infrastructure.DocumentModelling;

namespace Web.Infra.AutoMapper.Profiles
{
    public class GroupProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Grupo, GroupModel>()
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Nome))
                .ForMember(x => x.Color, o => o.MapFrom(m => m.Cor))
                .ForMember(x => x.ParentId, o => o.MapFrom(m => m.GrupoPai.Id));

            Mapper.CreateMap<DenormalizedReference<Grupo>, GroupModel>()
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Nome));
        }
    }
}