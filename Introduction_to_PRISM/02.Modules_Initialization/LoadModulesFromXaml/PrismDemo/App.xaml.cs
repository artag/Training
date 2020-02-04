using System;
using System.Reflection;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;

namespace PrismDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            // Конфигурация модуля описана в файле XamlCatalog.xaml.
            // На модуль reference не требуется. dll модуля кладется в нужную директорию.
            var uri = new Uri("/PrismDemo;component/XamlCatalog.xaml", UriKind.Relative);

            return ModuleCatalog.CreateFromXaml(uri);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
