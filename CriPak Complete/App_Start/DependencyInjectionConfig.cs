using Autofac;
using CriPakInterfaces;
using CriPakRepository.Parsers;

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
            builder.RegisterGeneric(typeof(Parser<>)).As(typeof(IParser<>)).InstancePerRequest();
            builder.RegisterType<TocParser>().As<ITocParser<IEndian>>().InstancePerRequest(); 
            builder.RegisterType<ItocParser>().As<IItocParser<IEndian>>().InstancePerRequest();
            builder.RegisterTypes(
                typeof(EtocParser),
                typeof(GtocParser)
            ).As<IParser<IEndian>>().InstancePerRequest();

        }
    }
}
