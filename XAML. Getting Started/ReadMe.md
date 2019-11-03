### Генерация иконок приложения

ЛКМ x 2 -> Package.appxmanifest -> Visual Assets -> Asset Generator
* `Source` = coffeeCup.png (рисунок для генерации логотипов, иконки и т.д.)
* `Tile background` = #333333 (фон загрузочного экрана)

Нажать кнопку `Generate`


### Убрать всплывающее отладочное меню для приложения

Tools -> Options -> Debugging -> General

Под пунктом "Enable UI Debugging Tools for XAML" убрать флажок напротив

`Show runtime tools in application`


### Know the XAML Layout Panels

* StackPanel
* Grid
* VariableSized WrapGrid (Только UWP)
* Relative Panel (Только UWP)
* Canvas

#### StackPanel

```xml
<StackPanel>
    <Rectangle Fill="LightBlue"
        Height="20" Margin="2"/>
    <Rectangle Fill="LightBlue"
        Height="20" Margin="2"/>
</StackPanel>
```

```xml
<StackPanel Orientation="Horizontal">
    <Rectangle Fill="LightBlue"
        Width="20" Margin="2"/>
    <Rectangle Fill="LightBlue"
        Width="20" Margin="2"/>
</StackPanel>2
```

#### Grid

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition/>
        <RowDefinition/>
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
        <ColumnDefinition/>
        <ColumnDefinition/>
    </Grid.ColumnDefinitions>
</Grid>
```

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition/>
        <RowDefinition/>
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="40"/>
        <ColumnDefinition/>
    </Grid.ColumnDefinitions>
    <Rectangle Fill="LightBlue"/>
</Grid>
```

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition/>
        <RowDefinition/>
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="40"/>
        <ColumnDefinition/>
    </Grid.ColumnDefinitions>
    <Rectangle Fill="LightBlue"
        Grid.Column="1"/>
</Grid>
```

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition/>
        <RowDefinition/>
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="40"/>
        <ColumnDefinition/>
    </Grid.ColumnDefinitions>
        <Rectangle Fill="LightBlue"
            Grid.Column="1" Grid.Row="1"/>
</Grid>
```

#### VariableSized WrapGrid (Только UWP)

```xml
<VariableSizedWrapGrid
    MaximumRowsOrColumns="3"
    ItemHeight="50" ItemWidth="50"
    Orientation="Vertical">
    <Rectangle Fill="Purple"/>
    <Rectangle Fill="Orange"
        VariableSizedWrapGrid.RowSpan="2"/>
    <Rectangle Fill="LightBlue"
        VariableSizedWrapGrid.RowSpan="2"
        VariableSizedWrapGrid.ColumnSpan="2"/>
</VariableSizedWrapGrid>
```

#### Relative Panel (Только UWP)

```xml
<RelativePanel>
    <TextBlock Text="Firstname" Margin="5"
        x:Name="labelFirstName"/>
    <TextBox Text="Thomas"
        RelativePanel.Below="labelFirstName"/>
</RelativePanel>
```

```xml
<RelativePanel>
    <TextBlock Text="Firstname" Margin="5"
        x:Name="labelFirstName"/>
    <TextBox Text="Thomas"
        RelativePanel.RightOf="labelFirstName"/>
</RelativePanel>
```

#### Canvas

```xml
<Canvas>
    <Rectangle Fill="LightBlue"
        Height="50" Width="50"
        Canvas.Left="50"
        Canvas.Top="100"/>
    <Rectangle Fill="Orange"
        Height="50" Width="50"
        Canvas.Left="75"
        Canvas.Top="125"/>
</Canvas>
```

```xml
<Canvas>
    <Rectangle Fill="LightBlue"
        Height="50" Width="50"
        Canvas.Left="50" Canvas.ZIndex="1"
        Canvas.Top="100"/>
    <Rectangle Fill="Orange"
        Height="50" Width="50"
        Canvas.Left="75"
        Canvas.Top="125"/>
</Canvas
```


### Understand the Size of Rows and Columns

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="100"/>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <Rectangle Fill="LightBlue"/>

    <Rectangle Fill="Orange"
        Grid.Row="2" Height="50"/>
</Grid>
```


### Use Layout Properties to Position Elements

```xml
<Grid Width="200" Height="200">
    <Button Content="OK"
        HorizontalAlignment="Left"
        VerticalAlignment="Top"/>
</Grid>
```

```xml
<Grid Width="200" Height="200">
    <Button Content="OK"
        HorizontalAlignment="Right"
        VerticalAlignment="Top"/>
</Grid>
```

```xml
<Grid Width="200" Height="200">
    <Button Content="OK"
        HorizontalAlignment="Stretch"
        VerticalAlignment="Bottom"/>
</Grid>
```

```xml
<Grid Width="200" Height="200">
    <Button Content="OK"
        HorizontalAlignment="Stretch"
        VerticalAlignment="Stretch"/>
</Grid>
```

```xml
<Grid Width="200" Height="200">
    <Button Content="OK"
        HorizontalAlignment="Stretch"
        VerticalAlignment="Stretch"
        Margin="50"/>
</Grid>
```

```xml
<Grid Width="200" Height="200">
    <Button Content="OK"
        HorizontalAlignment="Stretch"
        VerticalAlignment="Stretch"
        Margin="50 10 50 0"/>
</Grid>
```


### Set Attached Properties in XAML

Attribute Syntax:
```xml
<Button Content="OK"/>
```

Property Element Syntax:
```xml
<Button>
    <Button.Content>
        OK
    </Button.Content>
</Button>
```

Attribute Syntax:
```xml
<Button Grid.Row="1"/>
```

Property Element Syntax:
```xml
<Button>
    <Grid.Row>
        1
    </Grid.Row>
</Button>
```


### Set Attached Properties in C#

XAML:
```xml
<Button Grid.Row="1"/>
```

C#:
```csharp
var btn = new Button();
btn.SetValue(Grid.RowProperty, 1);

var column =
(int)btn.GetValue(Grid.RowProperty);
```


### Как отключить генерацию метода `Main`

Свойства проекта -> Build -> Conditional compilation symbols: вставить переменную
```
DISABLE_XAML_GENERATED_MAIN
```


### Create a 1:N Namespace Mapping (работает только для WPF)

Создание namespace для xaml элементов, содержащего в себе несколько namespace'ов
(наподобие default namespace: http://schemas.microsoft.com/winfx/2006/xaml/presentation).

В default namespace содержатся namespace'ы всех Control'ов, кистей и т.п.

Шаги.

**1**. В проекте, где определены нужные UserControl открыть файл `AssemblyInfo.cs` и вставить туда:
```
[assembly: XmlnsDefinition("https://www.thomasclaudiushuber.com/wpf",
    "WiredBrainCoffee.Controls.Common")]
[assembly: XmlnsDefinition("https://www.thomasclaudiushuber.com/wpf",
    "WiredBrainCoffee.Controls.Details")]
[assembly: XmlnsPrefix("https://www.thomasclaudiushuber.com/wpf", "thomas")]
```

Здесь:
* `"https://www.thomasclaudiushuber.com/wpf"` - уникальный namespace, который будет выступать
идентификатором для нескольких namespace.

* `WiredBrainCoffee.Controls.Common` и `WiredBrainCoffee.Controls.Details` - namespace'ы контролов.

* `thomas` - предлагаемый префикс (будет предлагать такой префикс по умолчанию - `xmlns:thomas`).

**2**. В xaml файле, где будет использоваться контролы:
```xml
xmlns:thomas="https://www.thomasclaudiushuber.com/wpf"
...
<thomas:NavigationControl/>
<thomas:CustomerDetailControl Grid.Column="1" />
```


### How the XAML Processor Converts Types

* In XAML every attribute value is by default a string
* XAML processor converts primitive types like bool, int, and double
* XAML processor converts enumeration values

*Пример 1*. Конвертация типов "из коробки"

XAML:
```xml
<Button
    Content="Add customer"
    Width="200"
    VerticalAlignment="Top"/>
```

C#
```csharp
var btn = new Button
{
    Content = "Add customer",
    Width = 200,
    VerticalAlignment = VerticalAlignment.Top
};
```

Кастомные конвертеры:

* UWP - Integrated in the XAML processor
* WPF - Specified with TypeConverters

*Пример 2*. Кастомная конвертация

XAML:
```xml
<Button
    Margin="10 5 0 0"
    Background="Black"/>
```

C#
```csharp
var btn = new Button
{
    Margin = new Thickness(10, 5, 0, 0),
    Background = new SolidColorBrush(Colors.Black)
};
```


### Ways to set the Customer property

* Property Element Syntax
* Content Syntax
* Attribute Syntax

Далее идут примеры установки значения Customer в `CustomerDetailControl` контрол различными
способами.

#### Property Element Syntax

В `MainPage.xaml`:
```xml
...
<!-- Customer Detail -->
<controls:CustomerDetailControl
    Grid.Row="1"
    Grid.Column="1"
    x:Name="customerDetailControl">
    <controls:CustomerDetailControl.Customer>
        <model:Customer FirstName="Thomas" LastName="Developer" IsDeveloper="True"/>
    </controls:CustomerDetailControl.Customer>
</controls:CustomerDetailControl>
```

Ничего более не надо делать.


#### Content Syntax

В `MainPage.xaml`:
```xml
...
<!-- Customer Detail -->
<controls:CustomerDetailControl
    Grid.Row="1"
    Grid.Column="1"
    x:Name="customerDetailControl">
        <model:Customer FirstName="Thomas" LastName="Developer2" IsDeveloper="True"/>
</controls:CustomerDetailControl>
```

В `CustomerDetailControl.xaml.cs` надо добавить атрибут для класса с указанием какой объект будет
использоваться для установки `Context`:
```csharp
[ContentProperty(Name = nameof(Model.Customer))]
public sealed partial class CustomerDetailControl : UserControl
{
}
...
```


#### Attribute Syntax

Будет рассмотрен в двух следующих разделах, отдельно для UWP и WPF.


### Convert from String to Customer in UWP

1. В `MainPage.xaml`:
```xml
...
<!-- Customer Detail -->
<controls:CustomerDetailControl
    Grid.Row="1"
    Grid.Column="1"
    x:Name="customerDetailControl"
    Customer="Thomas,Developer,true"/>
```

2. Создание конвертера `/Models/CustomerConverter.cs`:
```csharp
public static class CustomerConverter
{
    public static Customer CreateCustomerFromString(string inputString)
    {
        var values = inputString.Split(',');
        return new Customer
        {
            FirstName = values[0],
            LastName = values[1],
            IsDeveloper = bool.Parse(values[2]),
        };
    }
}
```

3. Добавление атрибута `CreateFromString(MethodName = ...)` в класс `Customer`:
```csharp
[CreateFromString(
    MethodName = "WiredBrainCoffee.CustomersApp.Model.CustomerConverter.CreateCustomerFromString")]
public class Customer : Observable
{
    ...
}
```
