using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using System.Text;
using Web.Models;
using Integer.Domain.Acesso;
using Web2.Models;

namespace Web.Infra.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Usuario, CurrentUserModel>()
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Nome))
                .ForMember(x => x.GroupId, o => o.MapFrom(m => m.GrupoId));
        }
    }
}