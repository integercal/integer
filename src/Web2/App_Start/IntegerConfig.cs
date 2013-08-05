using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using Autofac;
using Autofac.Integration.WebApi;
using Integer.Domain.Acesso;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Events;
using Integer.Infrastructure.IoC;
using Integer.Infrastructure.Repository;
using Raven.Client;
using Web.Infra.AutoMapper;
using Web.Security;
using Raven.Client.Extensions;
using Web.Infra;

[assembly: WebActivatorEx.PostApplicationStartMethod(
    typeof(Web.IntegerConfig), "PreStart")]

namespace Web
{
    public static class IntegerConfig
    {
        public const string RavenSessionKey = "RavenDB.Session";

        public static IDocumentSession CurrentSession
        {
            get
            {
                return (IDocumentSession)HttpContext.Current.Items[RavenSessionKey];
            }
            set
            {
                HttpContext.Current.Items[RavenSessionKey] = value;
            }
        }

        public static void PreStart()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public static void Register(HttpConfiguration config)
        {
            InitializeDocumentStore();
            ConfigIoC();
            ConfigAutoMapper();

            ConfigWebApi(config);
        }

        private static void InitializeDocumentStore()
        {
            DocumentStoreHolder.Initialize();
        }

        private static void ConfigIoC()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(c =>
            {
                //TODO: multi-tenancy (wait RavenHQ response)
                //var tenant = HttpContext.Current.Request.Headers["Host"].Split('.')[0];
                //DocumentStoreHolder.DocumentStore.DatabaseCommands.EnsureDatabaseExists(tenant);

                //if (CurrentSession == null)
                //    CurrentSession = DocumentStoreHolder.DocumentStore.OpenSession(tenant);

                if (CurrentSession == null)
                    CurrentSession = DocumentStoreHolder.DocumentStore.OpenSession();
                return CurrentSession;
            }).As<IDocumentSession>();

            builder.RegisterType<EventoRepository>().As<Eventos>();
            builder.RegisterType<GrupoRepository>().As<Grupos>();
            builder.RegisterType<LocalRepository>().As<Locais>();
            builder.RegisterType<UsuarioRepository>().As<Usuarios>();

            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<EventoCanceladoEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<ReservaDeLocalAlteradaEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<HorarioDeEventoAlteradoEvent>>();

            builder.Register<TrocaSenhaService>(c => new TrocaSenhaService(c.Resolve<Usuarios>()));

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            IoCWorker.Initialize(resolver);
        }

        private static void ConfigAutoMapper()
        {
            AutoMapperConfiguration.Configure();
        }

        private static void ConfigWebApi(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new BasicAuthenticationHandler());
            config.Filters.Add(new ExceptionFilter());
        }
    }
}