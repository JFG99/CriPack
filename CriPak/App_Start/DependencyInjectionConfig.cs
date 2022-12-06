using Autofac;
using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Repositories;
using FileRepository;
using MetaRepository;
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
            builder.RegisterTypes(
                 typeof(MetaReader)
            ).AsSelf().As<IReaderDetailsRepository<IDisplayList>>().InstancePerLifetimeScope(); 
            builder.RegisterTypes(
                  typeof(FileExtractor)
             ).AsSelf().As<IExtractorsRepository<IFiles>>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ReaderDetailRepository<,>)).As(typeof(ReaderDetailRepository<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ExtractorRepository<,,>)).As(typeof(ExtractorRepository<,,>)).InstancePerLifetimeScope();
            var assemblies = typeof(DependencyInjectionConfig).Assembly.GetReferencedAssemblies().Where(x => x.Name.EndsWith("Repository")).Select(Assembly.Load).ToArray();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IDetailMapper<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IExtractorMapper<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IWriter<>)).InstancePerLifetimeScope();
            builder.RegisterType<CpkMeta>().AsSelf().As<IMeta>().InstancePerLifetimeScope();
            builder.RegisterTypes(
                typeof(ContentHeader),
                typeof(TocHeader),
                typeof(EtocHeader),
                typeof(GtocHeader),
                typeof(ItocHeader)
            ).AsSelf().As<IHeader>().InstancePerLifetimeScope();
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
