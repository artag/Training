# Приемы разработки ASMX веб-сервисов

*Источник: https://habr.com/ru/post/254165/*

В этой статье я расскажу о различных приемах разработки SOAP веб-сервисов по
технологии ASMX, а также об этой технологии в целом.
Кроме SOAP, также будет рассмотрена реализация AJAX. Статья будет полезна
как тем, кто уже знаком с ней, так и тем, кто только собирается создать свой
первый веб-сервис.

## Историческая справка

С самого начала корпорация Microsoft была одним из основных разработчиков
стандарта SOAP. В 2002 году в составе самой первой версии ASP.NET 1.0 она
представила технологию ASMX (Active Server Method Extended), которая
позволила разработчикам в новейшей Visual Studio 2002 легко создавать и
потреблять SOAP веб-сервисы. Отмечу, что эта технология официально на MSDN
имеет название «XML Web Services». В те годы SOAP только делал первые
серьезные шаги в мире веб-разработки. Консорциум W3C одобрил SOAP 1.1 в
2000 году, SOAP 1.2 в 2003 году (дополнен в 2007 году). Поэтому было очень
важно сделать для нового стандарта легкую в освоении и применении
технологию. И эта цель была достигнута – чтобы работать с веб-сервисами,
разработчику даже не обязательно было знать XML, SOAP и WSDL.

В последующие годы технология ASMX получила очень широкое распространение
и признание. Также с самого начала Microsoft поставляла к ней аддон Web
Services Enhancements (WSE), который позволял реализовывать различные
спецификации безопасности WS-* такие, как WS-Security, WS-Policy,
WS-ReliableMessaging. Последняя версия — WSE 3.0 вышла в 2005 году. А в
2007 году в составе .NET 3.0 была представлена технология Windows
Communication Foundation (WCF), которая стала официальной заменой ASMX.
Несмотря на то, что технология ASMX уже давно не развивается, она
продолжает широко использоваться и поддерживается новейшими версиями .NET
Framework.

## ASMX и WCF

Почему же технология ASMX все еще так популярна? Все очень просто: она
легка в применении и прекрасно решает задачу в большинстве случаев.
Преимущество WCF проявляется, например, в тех случаях, когда вам нужна
высокая скорость транспорта, дуплекс, потоковая передача, соблюдение
современных стандартов безопасности, REST. Кстати, если вам нужен только
REST, то вместо WCF стоит использовать технологию ASP.NET Web API.

Перечислим конкретно плюсы каждой технологии:
*Плюсы ASMX:*
* Легкость в разработке
* Легкость в изучении
* Нет «ада» конфигурирования (http://stackoverflow.com/questions/7029815/wcf-configuration-hell)

*Плюсы WCF:*
* Очень разнообразные и гибкие возможности транспорта
* Актуальная и развивающаяся технология
* Различные варианты хостинга
* Возможность реализации большого множества стандартов WS

Итак, WCF – это «швейцарский нож» в области транспорта данных, а ASMX –
«добротная отвертка». И лучше всего, конечно, уметь пользоваться обоими
инструментами. Поскольку приемы разработки WCF в интернете описаны более
полно и актуально, я решил, что нужно написать статью про ASMX, которая
пригодится тем, кому приходится поддерживать старые веб-сервисы, и тем,
кто продолжает применять эту технологию для создания новых.

## Введение

В статье описаны 20 различных практических приемов, которые можно
применить при разработке веб-сервисов по данной технологии. Сценарий для
примеров будет следующий. Имеется регулярно пополняемая база данных
финансовых отчетов. Необходимо разработать универсальный механизм, с
помощью которого у различных клиентов всегда будут актуальные данные по
этим отчетам. Решение: пишем SOAP веб-сервис с двумя методами:

* Первый метод принимает период во времени и возвращает идентификаторы
всех отчетов, которые появились в этом периоде.

* Второй метод принимает идентификатор отчета и возвращает сами данные по
отчету.

Потребители веб-сервиса регулярно шлют запросы к первому методу,
указывая период с момента их последнего запроса,
и при наличии в ответе идентификаторов, запрашивают данные через второй метод.

Примеры демонстрируются на основе кода из «Рекомендуемой конструкции», и
чтобы их протестировать достаточно вызвать веб-метод GetReportInfo как
показано в примере «Прокси-класс».

## 1. Простейшая конструкция

*Пример: 01_Simple_example*

Начнем с описания простейшей конструкции веб-сервиса.
Внимание, пример носит исключительно теоретический характер!
Хоть он и рабочий, никогда так не делайте на практике.
Это только демонстрация простоты самой технологии ASMX.

* Новый проект "ASP.NET Empty Web Application" (или "ASP.NET Web Service Application")

* Проект FinReportWebService

Два файла:

* `FinReport.asmx`
```text
<%@ Class="FinReportWebService.FinReportService" %>
```

* `FinReportService.cs`
```csharp
public class FinReportService
{
    [WebMethod]
    public int[] GetReportIdArray(DateTime dateBegin, DateTime dateEnd)
    {
        return new[] { 357, 358, 360, 361 };
    }

    [WebMethod]
    public FinReport GetReport(int reportId)
    {
        return new FinReport
        {
            ReportId = reportId,
            Date = new DateTime(2015, 03, 15),
            Info = "Some Info"
        };
    }
}
```

* Запуск: `https://localhost:44386/FinReport.asmx`

### Объяснение

Веб-сервис представлен одним обычным классом с одной лишь обязательной
особенностью – некоторые его методы помечены специальным атрибутом `[WebMethod]`.

Такие методы класса становятся веб-методами веб-сервиса с соответствующей
сигнатурой вызова. Этот класс должен обладать конструктором по умолчанию.
При каждом новом запросе IIS его инстанциирует дефолтным конструктором и
вызывает соответствующий метод.

Вторая обязательная часть минимальной конструкции – это файл с
расширением `asmx`, внутри которого необходимо указать этот класс.

### Создание веб-сервиса с помощью VS

* Add New Item -> File `ExchangeRate.asmx`(тип Web Service)

Получившийся файл `ExchangeRate.asmx`:
```text
<%@ WebService Language="C#" CodeBehind="ExchangeRate.asmx.cs" Class="FinReportWebService.ExchangeRate" %>
```

Оператор `Language=«C#»` является рудиментарным, и нужен только если вы
будете писать исходный код непосредственно внутри `asmx` файла. Такой код
будет компилироваться динамически. Но я считаю, что в целом динамическая
компиляция веб-сервиса — не очень хорошая практика, и в частности, не
рекомендую использование специальной папки `App_Code`. А оператор
`CodeBehind="ExchangeRate.asmx.cs"` просто связывает два файла на уровне
Visual Studio.

## 2. Рекомендуемая конструкция

*Пример: 02_Recommended_example*

В этом примере тот же самый веб-сервис реализован более корректным
образом. Хотя это и более правильный код, он также служит только для
демонстрации. Например, здесь **пропущены** такие важные стандартные вещи
как **авторизация**, **обработка исключений**, **логирование**.
Также этот пример будет основой, на которой будут демонстрироваться
другие приемы этой статьи.

* Новый проект "ASP.NET Empty Web Application" (или "ASP.NET Web Service Application")

* Проект FinReportWebService

Два файла:

* `FinReport.asmx`
```text
<%@ Class="FinReportWebService.FinReportService" %>
```

* `FinReportService.cs`
```csharp
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[WebService(Description = "Фин. отчеты", Namespace = XmlNS)]
public class FinReportService : WebService
{
    public const string XmlNS = "http://asmx.report.ru/";

    [WebMethod(Description = "Получение списка ID отчетов по периоду")]
    public GetReportIdArrayResult GetReportIdArray(GetReportIdArrayArg arg)
    {
        return new GetReportIdArrayResult
        {
            ReportIdArray = new [] {357, 358, 360, 361}
        };
    }

    [WebMethod(Description = "Получение отчета по ID")]
    public GetReportResult GetReport(GetReportArg arg)
    {
        return new GetReportResult
        {
            Report = new FinReport
            {
                ReportId = arg.ReportId,
                Date = new DateTime(2015, 03, 15),
                Info = GetReportInfo(arg.ReportId)
            }
        };
    }

    private string GetReportInfo(int reportId)
    {
        return "ReportId = " + reportId;
    }
}

// [Serializable]
// [XmlType(Namespace = FinReportService.XmlNS)]
public class FinReport
{
    public int ReportId { get; set; }
    public DateTime Date { get; set; }
    public string Info { get; set; }
}

public class GetReportIdArrayArg
{
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
}

public class GetReportIdArrayResult
{
    public int[] ReportIdArray { get; set; }
}

public class GetReportArg
{
    public int ReportId { get; set; }
}

public class GetReportResult
{
    public FinReport Report { get; set; }
}
```

### Объяснение

Атрибут `[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]`
означает, что веб-сервис проверяется на соответствие спецификации WSI
Basic Profile 1.1. Например, согласно ней запрещена перегрузка имени
операции, или применение атрибута `[SoapRpcMethod]`. Такие нарушения
будут приводить к ошибке веб-сервиса "Служба
"FinReportWebService.FinReportService" не отвечает спецификации Simple
SOAP Binding Profile Version 1.0.". При отсутствии этого атрибута
нарушения будут приводить только к предупреждению "Эта веб-служба не
отвечает требованиям WSI Basic Profile v1.1.". В общем случае
**рекомендуется** добавлять этот атрибут, что обеспечивает большую
интероперабельность.

Атрибут `[WebService(Description = «Фин. отчеты», Namespace = XmlNS)]`
имеет всего три свойства:
* `Namespace` – дефолтный ХМЛ нэймспейс – указывать обязательно
* `Description` – описание веб-сервиса, отображаемое в браузере
* `Name` – имя веб-сервиса (по дефолту берется имя класса)

Наследование от класса `WebService` дает доступ к объектам `HttpContext`,
`HttpSessionState` и некоторым другим, что в некоторых случаях может быть
полезно.

В атрибуте `[WebMethod(Description = «Получение отчета по ID»)]` как
правило указывают только `Description`, который описывает веб-метод в
браузере, другие свойства используются редко.

Входящие параметры и возвращаемые значения рекомендуется инкапсулировать
в специальные классы. В примере классы названы с добавлением суффиксов
`-Arg` и `-Result` к названию метода, что означает аргумент и результат.
В этом примере для упрощения они все находятся в одном файле
`FinReportService.cs`, но в реальных проектах каждый из них надо
размещать в отдельном файле в специальной папке типа
`FinReportServiceTypes`. Также их удобно наследовать от общих классов.

По идее, ко всем собственным классам в веб-методах необходимо указывать
атрибуты `[Serializable]` и `[XmlType(Namespace = FinReportService.XmlNS)]`.
Однако в данном случае это не обязательно. Ведь если производится только
XML-сериализация, то атрибут `[Serializable]` не нужен, а XML нэймспейс и
так по умолчанию берется из атрибута `[WebService]`.

Замечание: в отличие от WCF в ASMX используется обычный `XmlSerializer`,
что позволяет широко управлять сериализацией с помощью таких стандартных
атрибутов как `[XmlType]`, `[XmlElement]`, `[XmlIgnore]` и т.д.

## 3. Прокси-класс с помощью wsdl.exe. Создание клиента

*Пример: 02_Recommended_example* *(тот же, что и предыдущий)*

Утилита `wsdl.exe` является соответствующей для asmx техникой
потребления SOAP веб-сервисов. По wsdl файлу или ссылке она
генерирует **прокси-класс** – специальной класс, максимально упрощающий
обращение к данному веб-сервису. Разумеется, не важно на какой
технологии реализован сам веб-сервис, это может быть что угодно —
ASMX, WCF, JAX-WS или NuSOAP. Кстати, у WCF аналогичная утилита
называется `SvcUtil.exe`.

Утилита расположена в папке
`C:\Program Files (x86)\Microsoft SDKs\Windows`, более того, она там
представлена в разных версиях, в зависимости от версии .net,
разрядности, версии Windows и Visual Studio.

*Примеры использования*
```
wsdl http://192.168.1.101:8080/SomeDir/SomeService?wsdl
wsdl HabraService.wsdl
```

### Создание клиента

* Новый проект клиента (в примере WPF) FinReportWebServiceClient.

* В этот проект:

  * добавить папку `ProxyClass`

  * в папку скопировать `wsdl.exe`

  * в папке создать батник `GenProxyClass.bat`:
  ```
    wsdl /n:FinReportWebServiceClient.ProxyClass https://localhost:44336/FinReport.asmx?wsdl
    pause
  ```

  * через Solution Explorer – Show All Files, включить все три файла в
  солюшен.

  * в Reference проекта добавить `System.Web.Services`

* На форму добавить пару кнопок, а в исходный код формы следующие три метода:
```csharp
private FinReportService GetFinReportService()
{
    const int TimeoutMilliSeconds = 100 * 1000;

    var service = new FinReportService();
    service.Url = "https://localhost:44336/FinReport.asmx";
    service.Timeout = TimeoutMilliSeconds;
    return service;
}

private void GetReportIdArray()
{
    using (var service = GetFinReportService())
    {
        var arg = new GetReportIdArrayArg();
        arg.DateBegin = new DateTime(2015, 03, 01);
        arg.DateEnd = new DateTime(2015, 03, 02);

        var result = service.GetReportIdArray(arg);
        MessageBox.Show($"result.ReportIdArray.Length = {result.ReportIdArray.Length}");
    }
}

private void GetReport()
{
    using (var service = GetFinReportService())
    {
        var arg = new GetReportArg();
        arg.ReportId = 45;

        var result = service.GetReport(arg);
        MessageBox.Show($"result.Report.Info = \"{result.Report.Info}\"");
    }
}
```

### Объяснение

* Для `*.bat` файла:

  * С помощью аргумента `/n:FinReportWebServiceClient.ProxyClass` мы
  указываем нэймспейс для класса.
  Запустив его, вы должны получить файл `FinReportService.cs`.

  * Запускать `GenProxyClass.bat` надо при работающем сервисе.

  * `https://localhost:44336/FinReport.asmx` (или что-то похожее) - это адрес сервиса.

* Для методов на форме:

  * Самыми важными свойствами прокси-класса являются `Url` и `Timeout`
  причем таймаут указывается в **миллисекундах** и 100 секунд это его
  дефолтное значение.

### Создание прокси-класса по wsdl файлу

*(в примере не показано)*

В случае создания прокси-класса по wsdl файлу, который ссылается на
внешние xsd схемы, все эти схемы необходимо перечислить в команде:

```
wsdl /n:MyNamespace HabraService.wsdl Data.xsd Common.xsd Schema.xsd
```

Однако кроме ручного создания прокси-класса Visual Studio позволяет его
создать автоматически. Пункт «Add Service Reference» позволяет создать
прокси-класс по технологии WCF, и там же в «Advanced» есть кнопка «Add
Web Reference», которая создает его уже по технологии ASMX.

## 4. Серверный класс по данному wsdl

*Пример: 03_WebService_By_Wsdl*

wsdl описание веб-сервиса в технологии ASMX генерируется автоматически.
Однако иногда возникает обратная задача: по данному wsdl файлу разработать
соответствующий ему веб-сервис. Решается она с помощью той же утилиты
`wsdl.exe`. Она может создать необходимый скелет из классов и вам останется
только реализовать программную логику веб-методов.

Для примера возьмем wsdl нашего веб-сервиса.
Сохраните его из браузера как файл `FinReport.wsdl` либо скопируйте отсюда:

Файл: `FinReport.wsdl`:
```xml
<?xml version="1.0" encoding="utf-8"?>
<wsdl:definitions xmlns:tm="http://microsoft.com/wsdl/mime/textMatching/" xmlns:soapenc="http://schemas.xmlsoap.org/soap/encoding/" xmlns:mime="http://schemas.xmlsoap.org/wsdl/mime/" xmlns:tns="http://asmx.report.ru/" xmlns:soap="http://schemas.xmlsoap.org/wsdl/soap/" xmlns:s="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://schemas.xmlsoap.org/wsdl/soap12/" xmlns:http="http://schemas.xmlsoap.org/wsdl/http/" targetNamespace="http://asmx.report.ru/" xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">
  <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Фин. отчеты</wsdl:documentation>
  <wsdl:types>
    <s:schema elementFormDefault="qualified" targetNamespace="http://asmx.report.ru/">
      <s:element name="GetReportIdArray">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="arg" type="tns:GetReportIdArrayArg" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="GetReportIdArrayArg">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="DateBegin" type="s:dateTime" />
          <s:element minOccurs="1" maxOccurs="1" name="DateEnd" type="s:dateTime" />
        </s:sequence>
      </s:complexType>
      <s:element name="GetReportIdArrayResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="GetReportIdArrayResult" type="tns:GetReportIdArrayResult" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="GetReportIdArrayResult">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="1" name="ReportIdArray" type="tns:ArrayOfInt" />
        </s:sequence>
      </s:complexType>
      <s:complexType name="ArrayOfInt">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="unbounded" name="int" type="s:int" />
        </s:sequence>
      </s:complexType>
      <s:element name="GetReport">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="arg" type="tns:GetReportArg" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="GetReportArg">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="ReportID" type="s:int" />
        </s:sequence>
      </s:complexType>
      <s:element name="GetReportResponse">
        <s:complexType>
          <s:sequence>
            <s:element minOccurs="0" maxOccurs="1" name="GetReportResult" type="tns:GetReportResult" />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:complexType name="GetReportResult">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="1" name="Report" type="tns:FinReport" />
        </s:sequence>
      </s:complexType>
      <s:complexType name="FinReport">
        <s:sequence>
          <s:element minOccurs="1" maxOccurs="1" name="ReportID" type="s:int" />
          <s:element minOccurs="1" maxOccurs="1" name="Date" type="s:dateTime" />
          <s:element minOccurs="0" maxOccurs="1" name="Info" type="s:string" />
        </s:sequence>
      </s:complexType>
    </s:schema>
  </wsdl:types>
  <wsdl:message name="GetReportIdArraySoapIn">
    <wsdl:part name="parameters" element="tns:GetReportIdArray" />
  </wsdl:message>
  <wsdl:message name="GetReportIdArraySoapOut">
    <wsdl:part name="parameters" element="tns:GetReportIdArrayResponse" />
  </wsdl:message>
  <wsdl:message name="GetReportSoapIn">
    <wsdl:part name="parameters" element="tns:GetReport" />
  </wsdl:message>
  <wsdl:message name="GetReportSoapOut">
    <wsdl:part name="parameters" element="tns:GetReportResponse" />
  </wsdl:message>
  <wsdl:portType name="FinReportServiceSoap">
    <wsdl:operation name="GetReportIdArray">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Получение списка ID отчетов по периоду</wsdl:documentation>
      <wsdl:input message="tns:GetReportIdArraySoapIn" />
      <wsdl:output message="tns:GetReportIdArraySoapOut" />
    </wsdl:operation>
    <wsdl:operation name="GetReport">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Получение отчета по ID</wsdl:documentation>
      <wsdl:input message="tns:GetReportSoapIn" />
      <wsdl:output message="tns:GetReportSoapOut" />
    </wsdl:operation>
  </wsdl:portType>
  <wsdl:binding name="FinReportServiceSoap" type="tns:FinReportServiceSoap">
    <soap:binding transport="http://schemas.xmlsoap.org/soap/http" />
    <wsdl:operation name="GetReportIdArray">
      <soap:operation soapAction="http://asmx.report.ru/GetReportIdArray" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetReport">
      <soap:operation soapAction="http://asmx.report.ru/GetReport" style="document" />
      <wsdl:input>
        <soap:body use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:binding name="FinReportServiceSoap12" type="tns:FinReportServiceSoap">
    <soap12:binding transport="http://schemas.xmlsoap.org/soap/http" />
    <wsdl:operation name="GetReportIdArray">
      <soap12:operation soapAction="http://asmx.report.ru/GetReportIdArray" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
    <wsdl:operation name="GetReport">
      <soap12:operation soapAction="http://asmx.report.ru/GetReport" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:service name="FinReportService">
    <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Фин. отчеты</wsdl:documentation>
    <wsdl:port name="FinReportServiceSoap" binding="tns:FinReportServiceSoap">
      <soap:address location="http://localhost:3500/FinReport.asmx" />
    </wsdl:port>
    <wsdl:port name="FinReportServiceSoap12" binding="tns:FinReportServiceSoap12">
      <soap12:address location="http://localhost:3500/FinReport.asmx" />
    </wsdl:port>
  </wsdl:service>
</wsdl:definitions>
```

* Создать в солюшене пустой web-проект с именем `FinReportWebServiceByWsdl`

* В новый проект:

  * Добавить папку `ServerClass`.

  * В папку скопировать файлы `FinReport.wsdl` и `wsdl.exe`. 

  * Здесь же создать bat файл `GenServerClass.bat`
  ```
  wsdl /server /n:FinReportWebServiceByWsdl.ServerClass FinReport.wsdl
  pause
  ```

* После запуска `GenServerClass.bat` мы должны получить файл
`FinReportService.cs`. Все четыре файла включить в солюшен.

* В корне проекта создать FinReportService.cs с реализацией абстрактных
методов из сгенеренного файла (пдробности см. ниже).

* Добавить файл `FinReportByWsdl.asmx`, который будет указывать на новый класс:
```
<%@ Class="FinReportWebServiceByWsdl.FinReportService" %>
```

### Объяснение

Единственное отличие от генерации прокси-класса – это атрибут `server` (в bat
файле). При этом создается абстрактный класс наследованный от `WebService` с
абстрактно описанными веб-методами.

Можно от него наследоваться, но при этом все равно придется копировать все
атрибуты, поэтому предлагаю сделать следующим образом.
Скопировать определение класса в новый файл и пространство имен, убрать слово
`abstract` и написать реализацию методов. На оставшиеся сгенеренные классы
в сгенеренном файле можно ссылаться.

После форматирования кода получился следующий файл. `FinReportService.cs`:
```csharp
using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

// Здесь остальные сгенеренные классы
using FinReportWebServiceByWsdl.ServerClass;

namespace FinReportWebServiceByWsdl
{
    [WebService(Namespace = "http://asmx.report.ru/")]
    [WebServiceBinding(Name = "FinReportServiceSoap", Namespace = "http://asmx.report.ru/")]
    public class FinReportService : WebService
    {
        [WebMethod]
        [SoapDocumentMethod(
            "http://asmx.report.ru/GetReportIdArray",
            RequestNamespace = "http://asmx.report.ru/",
            ResponseNamespace = "http://asmx.report.ru/",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public GetReportIdArrayResult GetReportIdArray(GetReportIdArrayArg arg)
        {
            return new GetReportIdArrayResult
            {
                ReportIdArray = new []{ 23, 666, 42 }
            };
        }

        [WebMethod]
        [SoapDocumentMethod(
            "http://asmx.report.ru/GetReport",
            RequestNamespace = "http://asmx.report.ru/",
            ResponseNamespace = "http://asmx.report.ru/",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public GetReportResult GetReport(GetReportArg arg)
        {
            return new GetReportResult
            {
                Report = new FinReport
                {
                    ReportID = arg.ReportID,
                    Date = new DateTime(2015, 03, 15),
                    Info = "ByWSDL"
                }
            };
        }
    }
}
```

## 5. ajax