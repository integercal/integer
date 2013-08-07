using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using System.Text;
using Web.Models;
using Integer.Domain.Acesso;
using Web2.Models;
using Integer.Domain.Paroquia;

namespace Web.Infra.AutoMapper.Profiles
{
    public class ConfigProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ParoquiaConfig, ConfigModel>()
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Nome))
                .ForMember(x => x.Priest, o => o.MapFrom(m => m.Paroco))
                .ForMember(x => x.Birth, o => o.MapFrom(m => m.DataCriacao.ToString("dd/MM/yyyy")))
                .ForMember(x => x.Telephone, o => o.MapFrom(m => m.Telefone))
                .ForMember(x => x.Address, o => o.MapFrom(m => m.Endereco))
                .ForMember(x => x.ScheduleStart, o => o.MapFrom(m => m.Configuracao.AgendamentoInicio.ToString("dd/MM/yyyy")))
                .ForMember(x => x.ScheduleEnd, o => o.MapFrom(m => m.Configuracao.AgendamentoFim.ToString("dd/MM/yyyy")))
                .ForMember(x => x.ReserveInterval, o => o.MapFrom(m => m.Configuracao.IntervaloReserva))
                .ForMember(x => x.Version, o => o.MapFrom(m => m.Plano.Versao))
                .ForMember(x => x.Since, o => o.MapFrom(m => m.Plano.DataAdesao.ToString("dd/MM/yyyy")));

        }
    }
}