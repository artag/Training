using System.Windows;
using Prism.Ioc;
using Prism.Modularity;

namespace ModuleA
{
    // Задает имя модуля, режим загрузки "As available".
    // [Module(ModuleName = "NameModuleA")]
    //
    // Задает имя модуля, режим загрузки "On demand".
    // [Module(ModuleName = "NameModuleA", OnDemand = true)]
    //
    // Зависимости модуля.
    // [ModuleDependency("moduleName")]
    [Module(ModuleName = "NameModuleA")]
    public class ModuleAModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            MessageBox.Show("ModuleA Loaded", "Information");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
