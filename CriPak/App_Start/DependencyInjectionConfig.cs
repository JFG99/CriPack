using Autofac;
using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Mappers;
using CriPakRepository.Parsers;
using CriPakRepository.Readers;
using CriPakRepository.Repositories;
using System.Linq;
using System.Reflection;

namespace CriPakComplete.App_Start
{
    public class DependencyInjectionConfig
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
            RegisterTypes(builder);
            return builder.Build();
        }

        private static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<Orchestrator>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MetaRepository>().AsSelf().As<IReaderDetailsRepository<IHeader>>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ReaderDetailRepository<,>)).As(typeof(ReaderDetailRepository<,>)).InstancePerLifetimeScope();
            var assemblies = typeof(DependencyInjectionConfig).Assembly.GetReferencedAssemblies().Where(x => x.Name.EndsWith("Repository")).Select(Assembly.Load).ToArray();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IDetailMapper<>)).InstancePerLifetimeScope();
            builder.RegisterType<CpkMapper>().As<CpkMapper>().InstancePerLifetimeScope();
            builder.RegisterType<CpkHeader>().AsSelf().As<IHeader>().InstancePerLifetimeScope();
            //builder.RegisterType(typeof(ParserRepository)).As(typeof(IParserRepository)).InstancePerRequest();
            //builder.RegisterTypes(
            //    typeof(EtocParser),
            //    typeof(GtocParser),
            //    typeof(ItocParser),
            //    typeof(TocParser)
            //).As<IParserRepository>().InstancePerRequest();

        }
    }
}
