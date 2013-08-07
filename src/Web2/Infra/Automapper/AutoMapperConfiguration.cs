using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Infra.AutoMapper.Profiles;
using AutoMapper;

namespace Web.Infra.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.AddProfile(new EventoProfile());
            Mapper.AddProfile(new UserProfile());
            Mapper.AddProfile(new LocalProfile());
            Mapper.AddProfile(new GroupProfile());
            Mapper.AddProfile(new ConfigProfile());
        }
    }
}