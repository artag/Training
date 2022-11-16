using Autofac;
using Clean.Architecture.ApplicationServices.Interfaces;
using Clean.Architecture.ApplicationServices.Services;

namespace Clean.Architecture.ApplicationServices;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}
