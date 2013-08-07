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

            Mapper.CreateMap<Usuario, UserModel>()
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Nome))
                .ForMember(x => x.Active, o => o.MapFrom(m => m.Ativo))
                .ForMember(x => x.GroupId, o => o.MapFrom(m => m.GrupoId))
                .ForMember(x => x.DateCreation, o => o.MapFrom(m => m.DataCriacao))
                .ForMember(x => x.Gender, o => o.MapFrom(m => m.Genero));
        }
    }
}