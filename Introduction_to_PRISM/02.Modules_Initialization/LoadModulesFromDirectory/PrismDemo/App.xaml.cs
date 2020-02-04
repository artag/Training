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
            // На модуль reference не требуется. dll модуля кладется в нужную директорию.
            return new DirectoryModuleCatalog()
            {
                ModulePath = @".\Modules",
            };
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
