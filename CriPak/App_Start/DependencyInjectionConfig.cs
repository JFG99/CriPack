using Autofac;
using CriPakInterfaces;
using CriPakRepository.Parsers;
using CriPakRepository.Repositories;

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
