﻿using System.Windows;
using Prism.Ioc;
using Prism.Modularity;

namespace ModuleA
{
    public class ModuleAModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            MessageBox.Show("ModuleA initialized", "Info");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
