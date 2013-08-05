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
    public class LocalProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Local, LocalModel>()
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Nome));

            Mapper.CreateMap<DenormalizedReference<Local>, LocalModel>()
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Nome));
        }
    }
}