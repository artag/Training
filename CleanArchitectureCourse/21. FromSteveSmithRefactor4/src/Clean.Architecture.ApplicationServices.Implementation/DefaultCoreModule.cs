using Autofac;
using Clean.Architecture.ApplicationServices.Implementation.Services;
using Clean.Architecture.ApplicationServices.Interfaces;

namespace Clean.Architecture.ApplicationServices.Implementation;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}
