# Getting Started with Prism


## Intro (Bootstrapper and Shell)

### The Building Blocks

* Shell
* Regions
* Modules
* Views
* Bootstrapper

### Bootstrapper

* Initializes application
* Core services
* Application specific services

#### Core Services

* `IModuleManager`
* `IModuleCatalog`
* `IModuleInitializer`
* `IRegionManager`
* `IEventAggregator`
* `ILoggerFacade`
* `IServiceLocator`

#### Bootstrapper Process

1. LoggerFacade
2. ModuleCatalog
3. Container
4. Region Adapter Mappings
5. Region Behaviors
6. Exception Types
7. Create Shell
8. Initialize Shell
9. Initialize Modules

### What is the Shell?

* Main Window/Page
* "Master Page"
* Contains **Regions**


## Regions

### What are Regions

* "Placeholder" for dynamic content
* No knowledge of views
* Create in code or in XAML
* Implements `IRegion`

### Region Manager

* Maintains collection of regions
* Provides a RegionName attached property
* Maps RegionAdapter to controls
* Provides a RegionContext attached property

### Region Adapters

#### ContentControlRegionAdapter

**ContentControl** - элемент управления с отдельным содержимым любого типа.

Наследование:
```
Object -> DispatcherObject -> DependencyObject -> Visual ->
-> UIElement -> FrameworkElement -> Control -> ContentControl
```

Производные классы (Region может быть установлен в объекты след. классов):
```
System.Activities.Presentation.WorkflowElementDialog
System.Activities.Presentation.WorkflowItemPresenter
System.Activities.Presentation.WorkflowItemsPresenter
System.Activities.Presentation.WorkflowViewElement
System.Activities.Presentation.View.ExpressionTextBox
System.Activities.Presentation.View.TypePresenter
System.Windows.Window
System.Windows.Controls.DataGridCell
System.Windows.Controls.Frame
System.Windows.Controls.GroupItem
System.Windows.Controls.HeaderedContentControl
System.Windows.Controls.Label
System.Windows.Controls.ListBoxItem
System.Windows.Controls.ScrollViewer
System.Windows.Controls.ToolTip
System.Windows.Controls.UserControl
System.Windows.Controls.Primitives.ButtonBase
System.Windows.Controls.Primitives.StatusBarItem
System.Windows.Controls.Ribbon.RibbonControl
System.Windows.Controls.Ribbon.RibbonGalleryItem
System.Windows.Controls.Ribbon.RibbonTabHeader
```

#### ItemsControlRegionAdapter

**ItemsControl** - элемент управления, который может быть использован для представления коллекции элементов.

Наследование:
```
Object -> DispatcherObject -> DependencyObject -> Visual ->
-> UIElement -> FrameworkElement -> Control -> ItemsControl
```

Производные классы (Region может быть установлен в объекты след. классов):
```
System.Windows.Controls.HeaderedItemsControl
System.Windows.Controls.TreeView
System.Windows.Controls.Primitives.DataGridCellsPresenter
System.Windows.Controls.Primitives.DataGridColumnHeadersPresenter
System.Windows.Controls.Primitives.MenuBase
System.Windows.Controls.Primitives.Selector
System.Windows.Controls.Primitives.StatusBar
System.Windows.Controls.Ribbon.RibbonContextualTabGroupItemsControl
System.Windows.Controls.Ribbon.RibbonControlGroup
System.Windows.Controls.Ribbon.RibbonGallery
System.Windows.Controls.Ribbon.RibbonQuickAccessToolBar
System.Windows.Controls.Ribbon.RibbonTabHeaderItemsControl
```

#### SelectorRegionAdapter

**Selector** - элемент управления, позволяющий пользователю выбрать один из его дочерних элементов.

Наследование:
```
Object -> DispatcherObject -> DependencyObject -> Visual ->
-> UIElement -> FrameworkElement -> Control -> ItemsControl -> Selector
```

Производные классы (Region может быть установлен в объекты след. классов):
```

System.Windows.Controls.ComboBox
System.Windows.Controls.ListBox
System.Windows.Controls.TabControl
System.Windows.Controls.Primitives.MultiSelector
System.Windows.Controls.Ribbon.Ribbon
```

#### TabControlRegionAdapter (Silverlight only)

### Custom Region

*Пример см. в `StackPanelRegionAdapter`.*

* Derive from `RegionAdapterBase<T>`
* Implement `CreateRegion` method
  * `SingleActiveRegion` - Region that allows a maximum of one active view at a time.
  * `AllActiveRegion` - Region that keeps all the views in it as active. Deactivation of views is not allowed.
  * `Region` - Implementation of `IRegion` that allows multiple active views.
* Implement `Adapt` method
* Register your adapter

