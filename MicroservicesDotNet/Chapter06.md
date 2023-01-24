# 6. Data ownership and data storage. (Владение данными и их хранение)

- Which data microservices store.
(Какие данные хранят микросервисы).

- Understanding how data ownership follows business capabilities.
(Понимание того, как владение данными соответствует бизнес-функциям).

- Using data replication for speed and robustness.
(Использование репликации данных для повышения скорости и надежности).

- Building read models from event feeds with event subscribers.
(Построение read models из каналов (лент) событий с помощью подписчиков на события).

- Implementing data storage in microservices.
(Реализация хранения данных в микросервисах).

## 6.1 Each microservice has a data store

Микросервис может хранить три типа данных:

- Данные, относящиеся к возможностям, реализуемым микросервисом. Это данные, за которые
отвечает микросервис и которые он должен сохранять в безопасности и актуальном состоянии.

- События, созданные микросервисом. Во время обработки команд микросервису может потребоваться
инициировать события, чтобы информировать остальную часть системы об обновлениях
своих данных.

- Read models, основанные на данных в событиях из других микросервисов или иногда
на данных из запросов к другим микросервисы.

Эти три типа данных могут храниться в разных базах данных и даже в разных
типах баз данных.

## 6.2 Partitioning data between microservices. (Разделение данных между микросервисами)

Решение о том где хранить данные в микросервисной системе определяется двумя факторами:

- *Ownership of data* (владение данными) означает ответственность за сохранность,
корректность и актуальность данных.

- *Locality of data* (местонахождение данных) - где хранятся данные, необходимые микросервису.
Часто данные должны храниться поблизости - предпочтительно в самом микросервисе.

Эти два фактора могут находиться в противоречии, и для того, чтобы угодить обоим, часто
приходится хранить данные в нескольких местах. Это нормально, но важно, чтобы только одно из этих
мест считалось authoritative (авторитетным/официальным) источником.

В то время как один микросервис хранит достоверную копию части данных, другие микросервисы могут
mirror эти данные в их собственных хранилищах данных (рис 6.1):

![Microservices A and C can store mirrors of the data owned by microservice B.](images/33_data_mirror.jpg)

### 6.2.1 Rule 1: Ownership of data follows business capabilities. (Владение данными соответствует бизнес-функциям)

Первое правило - владение данными соответствует бизнес функциям. В [главе 4](Chapter04.md),
описано, что бизнес-функции определяют границы микросервиса - все функции должны быть
реализованы в одном микросервисе. Это включает в себя хранение данных, которые подпадают под
бизнес-функции.

DDD учит, что некоторые сущности (концепции) могут присутствовать в нескольких бизнес-функциях
и что значение сущностей может незначительно отличаться. Например, несколько микросервисов
могут иметь сущность клиента, и они будут работать с объектами клиента и хранить
их. Данные, хранящиеся в разных микросервисах, могут частично совпадать, но важно четко
представлять, какой микросервис отвечает за их актуальность.

Например, домашний адрес клиента должен принадлежать только одному микросервису.
Другой микросервис мог бы владеть историей покупок клиента, а третий - настройками уведомлений
клиента. Способ решить, какой микросервис отвечает за какой фрагмент данных (например, домашний
адрес клиента) - это выяснить, какая бизнес-функция (а значит и какой микросервис) поддерживает
эти данные в актуальном состоянии.

Давайте еще раз рассмотрим сайт электронной коммерции из глав [1](Chapter01.md) и
[2](Chapter02.md). На рис. 6.2 показан обзор того, как эта система обрабатывает запросы
пользователей на добавление товара в корзину покупок:

![Partitioning data between microservices.](images/34_partitioning_data.jpg)

Каждый микросервис на рис. 6.2 реализует свои бизнес-функции:

- shopping cart отвечает за отслеживание корзины покупок пользователей.
- product catalog отвечает за предоставление остальной части системы доступа к информации из
каталога продуктов.
- recommendations отвечает за расчет и предоставление рекомендаций по продуктам пользователям
сайта электронной коммерции.

Рис. 6.3 показывает данные, которыми владеет каждый из микросервисов.
Утверждение, что микросервис владеет частью данных, означает, что он должен хранить эти данные
и быть authoritative источником для этой части данных.

![Each microservice owns the data belonging to the business capability it implements.](images/35_data_belonging.jpg)

### 6.2.2 Rule 2: Replicate for speed and robustness. (Копируйте для скорости и надежности)

Вторым фактором, определяющим место хранения части данных в микросервисах является *locality*.
Существует большая разница между микросервисом, запрашивающим данные в своей собственной базе
данных, и микросервисом, запрашивающим те же данные у другого микросервиса.

Как только вы определитесь с владельцем данных, то обнаружите, что микросервисам необходимо
запрашивать данные друг у друга. Один микросервис, запрашивающий данные у другого,
создает связь между ними. Если второй микросервис не работает или работает медленно, пострадает
первый микросервис.

Чтобы ослабить эту связь, можно кэшировать ответы на запросы. Иногда вы кэшируете
ответы как они есть, но в других случаях вы можете сохранить read model, основанную
на ответах на запрос. В обоих случаях надо решить, когда и как кэшированный фрагмент данных
становится недействительным. Микросервис, которому принадлежат данные, решает,
когда часть данных все еще действительна, а когда она стала недействительной.
Endpoints у которых запрашиваются данные должны включать в ответ заголовки кэша,
сообщающие клиенту, как долго он должен кэшировать данные ответа.

#### Using HTTP cache headers to control caching. (Использование HTTP заголовков для управления кэшированием)

HTTP определяет ряд заголовков, которые можно использовать для управления кэшированием HTTP-ответов. Назначение механизмов кэширования HTTP:

- Исключить необходимость запрашивать информацию, которая уже есть у вызывающего абонента.
- Исключить необходимость отправлять полные HTTP-ответы.

Для исключения необходимости запрашивания информации, которая уже есть у вызывающего абонента,
сервер может добавить заголовок `cache-control` к своим ответам. Спецификация HTTP определяет
элементы, которые могут быть установлены в заголовке `cache-control`.
Наиболее распространенными являются директивы:

- `private | public` - кто может кэшировать ответ, только клиент или посредники (прокси—серверы)

- `max-age` - количество секунд, в течение которых ответ может быть кэширован
(данные могут считаться актуальными).

Пример заголовка cache-control, где разрешено кэширование только вызывающим, ответ
действителен в течение 3600 секунд:

```text
cache-control: private, max-age:3600
```

Другими словами, вызывающий может повторно использовать ответ в любое время, когда он захочет
выполнить HTTP запрос на тот же URL с тем же методом - `GET`, `POST`, `PUT`, `DELETE` в течение
3600 секунд.

Чтобы исключить необходимость отправки полного ответа в случаях, когда вызывающий абонент имеет
кэшированный, но устаревший ответ, могут быть использованы заголовки `ETag` и `If-None-Match`:
сервер может добавлять заголовок `ETag` к ответам. Это идентификатор для ответа.
Когда вызывающий делает более поздний запрос к тому же URL-адресу, используя тот же метод и
то же тело, он может включить `ETag` в заголовок запроса с именем `If-None-Match`.
Сервер может прочитать `ETag` и узнать, какой ответ был кэширован вызывающим абонентом.
Если сервер решит, что кэшированный ответ все еще действителен, он может вернуть ответ
с кодом состояния "304 Not Modified", разрешающий клиенту использовать уже кэшированный ответ.
Более того, сервер может добавить заголовок `cache-control` к ответу 304, чтобы продлить
период, в течение которого ответ может быть кэширован. Обратите внимание, что `ETag`
устанавливается сервером и позже снова считывается тем же сервером.

Рассмотрим микросервисы на рис. 6.3. Микросервис корзины покупок использует информацию о
товаре, запрашивая микросервис каталога товаров. Каталог товаров должен добавлять заголовки
кэша к своим ответам, а микросервис корзины покупок должен использовать их для определения
времени кэширования ответа. На рис. 6.4 показана последовательность запросов к каталогу
товаров, которые делает корзина покупок:

![Cache responses.](images/36_cache_control.jpg)

В ответе на первый запрос время кэширования ответа 3600 секунд. Во второй раз, когда корзина
покупок хочет сделать тот же запрос, кэшированный ответ используется повторно (прошло менее
3600 секунд). В третий раз запрос к каталогу товаров выполняется из-за того, что прошло более
3600 секунд.
Этот запрос включает `ETag` из первого ответа в заголовке `If-None-Match`. Каталог товаров
использует `ETag`, решает, что ответ останется прежним, и поэтому отправляет обратно более
короткий ответ "304 Not Modified" вместе с новым заголовком `cache-control` разрешающий
кэширование на дополнительные 1800 секунд.

#### Using read models to mirror data owned by other microservices. (Использование read models для зеркалирования данных, принадлежащих другим микросервисам)

Часто можно заменить запрос к другому микросервису запросом к собственной базе данных
микросервиса, создав read model. Это отличается от модели, используемой для хранения данных,
принадлежащих микросервису, где целью является хранение достоверной копии данных и
имеется возможность легко при необходимости их обновлять.

Данные для read models - сохраняются как последствия изменений где-то в другом месте.
Read models часто основаны на событиях из других микросервисов. Один микросервис
подписывается на события другого и обновляет свою собственную модель
данных о событиях по мере их поступления.

Read models также могут быть построены на основе ответов на запросы к другим микросервисам.
В этом случае время жизни данных в модели чтения определяется заголовками кэша в
этих ответах. Разница между обычным кэшированием и read model заключается в том, что для
построения read model данные в ответах преобразуются и, возможно дополняются, чтобы сделать
последующее чтение легким и эффективным.

Рассмотрим пример. Микросервис корзины покупок публикует события каждый раз, когда товар
добавляется или удаляется из корзины. На рис. 6.5 показан микросервис
shopper tracking, который подписывается на эти события и обновляет read model на их основе.

![The shopper tracking microservice subscribes to events.](images/37_tracking_events.jpg)

События, публикуемые микросервисом корзины покупок, сами по себе не являются хорошим
источником если мы хотим узнать, как часто товар был добавлен или удален из корзины покупок.
Но события являются хорошим источником, на основе которого можно построить такую модель.
Микросервис shopper tracking хранит два счетчика для каждого товара: первый - сколько раз товар
добавлялся в корзину, второй - сколько раз он был удален.
Каждый раз, когда из корзины покупок приходит событие, обновляется один из счетчиков.
Каждый раз, когда делается запрос о продукте, для этого продукта считываются оба счетчика.

### 6.2.3 Where does a microservice store its data? (Где микросервис хранит свои данные?)

Микросервис может использовать одну, две или более баз данных. Некоторые данные, хранящиеся в
микросервисе, могут хорошо вписываться в один тип базы данных, а другие данные могут лучше вписываться в другой. Существует несколько категорий баз данных (здесь далеко не все):

- relational databases
- key/value stores
- document databases
- column stores
- graph databases

На выбор вида/типа базы данных для микросервиса может повлиять:

- В какой форме находятся ваши данные? Хорошо ли они вписываются в реляционную модель,
document model или это key/value store, или это graph?

- What are the write scenarios? How much data is written? Do the writes come in
bursts, or are they evenly distributed over time?

- What are the read scenarios? How much data is read at a time? How much is
read altogether? Do the reads come in bursts?

- Сколько данных записывается по сравнению с тем, сколько считывается?

- Какие базы данных команда уже знает. Знает как разрабатывать на их основе и запускать в
производство?

Задавая себе эти вопросы - вы не только сможете выбрать подходящую базу данных,
но и, вероятно, улучшите понимание нефункциональных качеств, ожидаемых от микросервиса:

- насколько надежным должен быть микросервис
- какой объем нагрузки он должен обрабатывать
- как выглядит нагрузка
- насколько допустима задержка
- и так далее

Не следует слишком долго выбирать тип используемой БД. Лучше быстро выбрать более-менее
подходящий тип БД и запустить микросервис в производство, а на более позднем этапе,
возможно, заменить микросервис на другой, использующий другой тип БД.

## 6.3 Implementing data storage in a microservice. (Реализация хранения данных в микросервисе)

В этой главе будут рассмотрены следующие технологии:

- `SQL Server` - БД от Microsoft.
- [`Dapper`](https://github.com/StackExchange/dapper-dot-net) - A lightweight object-relational
mapper (ORM).
- [`EventStoreDB`](https://www.eventstore.com/eventstoredb) - A database product specifically
designed to store events.

#### Про Dapper

Dapper - это простая библиотека для работы с данными в базе данных SQL из C#.
Такие библиотеки называют `micro ORMs`. Аналоги - `Simple.Data` и `Massive`.
Эти библиотеки просты в использовании и быстрые.

Более традиционные ORM генерируют весь SQL, необходимый для чтения и записи данных в БД.
Для запросов в Dapper требуется писать SQL самому.

Использование Dapper в микросервисах иногда более предпочтительно, чем
использование полноценных ORM, таких как Entity Framework или NHibernate. Часто база данных для
микросервиса проста, и в таких случаях проще всего добавить тонкий слой, подобный Dapper.
Dapper помимо MSSQL может также работать с другими базами данных SQL, такими как PostgreSQL,
MySQL или Azure SQL.

#### EventStoreDB - a dedicated event database

EventStoreDB - это сервер базы данных с открытым исходным кодом, разработанный специально
для хранения событий. EventStoreDB хранит события в виде документов JSON.
EventStoreDB широко используется и зарекомендовал себя в сценариях с большой нагрузкой.

Помимо хранения событий, EventStoreDB имеет средства для чтения и подписки на них.
Например, EventStoreDB предоставляет свои собственные каналы событий - такие как ATOM
каналы (feeds), на которые могут подписаться клиенты.

EventStoreDB для работы с ним предоставляет HTTP API - для хранения, чтения и подписки на
события. Существует ряд клиентских библиотек EventStoreDB на разных языках,
включая C#, F#, Java, Scala, Erlang, Haskell и JavaScript, которые упрощают
работу с этой БД.

### 6.3.1 Preparing a development setup

Run the SQL Server on `localhost`, in a Docker container.

1. Pull down the latest SQL Server docker image:

```text
docker pull mcr.microsoft.com/mssql/server
```

2. Запуск MSSQL server в docker:

```text
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Some_password!' -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

- `-e` - переменная окружения
- `-p 1433:1433` - container exposes port 5001 and listens to traffic on
that port. Any incoming traffic to port 5001 is forwarded to port 80 inside the
container.
- `-d` - запуск docker контейнера в фоновом режиме и возврат обратно в терминал.

Примечания:

- Если SQL Server не запускается, возможно надо использовать двойные кавычки вместо одинарных.
- Пароль к MSSQL серверу должен быть минимум из 8 символов и сложным
(большие и малые буквы, цифры, символы).
- При таком запуске сервера БД в docker данные в БД не сохраняются после выключения контейнера.

1. Проверка, что SQL Server запущен:

```text
docker ps
```

4. Остановка контейнера:

```text
docker stop container_ID
```

### 6.3.2 Storing data owned by a microservice. (Хранение данных микросервиса)

Хранилище данных принадлежит исключительно самому микросервису и к нему осуществляется доступ
только через этот микросервис.

В качестве примера рассмотрим микросервис shopping cart (корзина покупок).
Реализация микросервиса shopping cart была частично сделана в [главе 2](Chapter02.md).
Будем использовать SQL Server, а для доступа к нему - Dapper.

Шаги по реализации хранения для микросервиса shopping cart:

1. Создание базы данных.
2. Использование Dapper в коде для: чтения, записи и обновления

#### Создание БД

Используемые таблицы (2 шт.):

`ShoppingCart`:

| PK | ID     |
|--- |--------|
|    | UserId |

`ShoppingCartItem`

| PK | ID                 |
|--- |--------------------|
| FK | ShoppingCartId     |
|    | ProductCatalogId   |
|    | ProductName        |
|    | ProductDescription |
|    | Amount             |
|    | Currency           |

Создание таблиц в БД.
Можно использовать SQL Management Studio и Visual Studio Code.
Коннектимся к `localhost`, порт 1433, пользователь `sa`, пароль `Some_password!`
(из параметров запуска). Файл
[create-shopping-cart-db.sql](chapter06/ShoppingCart/database-scripts/create-shopping-cart-db.sql):

```sql
CREATE DATABASE ShoppingCart
GO

USE [ShoppingCart]
GO

CREATE TABLE dbo.ShoppingCart (
    ID int IDENTITY(1,1) PRIMARY KEY,
    UserId bigint NOT NULL,
    CONSTRAINT ShoppingCartUnique UNIQUE(ID, UserId)
)
GO

CREATE INDEX ShoppingCart_UserId
ON [dbo].[ShoppingCart] (UserId)
GO

CREATE TABLE dbo.ShoppingCartItem (
    ID int IDENTITY(1,1) PRIMARY KEY,
    ShoppingCartId int NOT NULL,
    ProductCatalogId bigint NOT NULL,
    ProductName nvarchar(100) NOT NULL,
    ProductDescription nvarchar(500) NULL,
    Amount int NOT NULL,
    Currency nvarchar(5) NOT NULL
)
GO

ALTER TABLE dbo.ShoppingCartItem
WITH CHECK ADD CONSTRAINT FK_ShoppingCart FOREIGN KEY (ShoppingCartId)
REFERENCES dbo.ShoppingCart (ID)
GO

ALTER TABLE dbo.ShoppingCartItem
CHECK CONSTRAINT FK_ShoppingCart
GO

CREATE INDEX ShoppingCartItem_ShoppingCartId
ON dbo.ShoppingCartItem (ShoppingCartId)
GO
```

#### Использование Dapper

Проект [chapter06/ShoppingCart](chapter06/ShoppingCart/)

Теперь надо добавить NuGet пакет `Dapper` в микросервис `ShoppingCart` (запускать из директории,
где находится `ShoppingCart.csproj`):

```text
dotnet add package dapper
```

Dapper - это простой инструмент, который предоставляет несколько удобных методов расширения на
`IDbConnection` для упрощения работы с SQL в C#. Он также предоставляет некоторые базовые
возможности mapping. Например, когда строки, возвращаемые SQL-запросом, имеют имена столбцов,
равные именам свойств в классе, Dapper может автоматически делать map на экземпляры класса.

Еще надо добавить в `ShoppingCart` nuget-пакет `System.Data.SqlClient` (для доступа к MSSQL).

Изменим `IShoppingCart` - сделаем асинхронными вызовы к БД:

```csharp
public interface IShoppingCartStore
{
    Task<ShoppingCart> Get(int userId);
    Task Save(ShoppingCart shoppingCart);
}
```

Реализация чтения из БД [`ShoppingCartStore`](chapter06/ShoppingCart/ShoppingCart/ShoppingCartStore.cs):

```csharp
// (1) - Connection string to the ShoppingCart database in the MSSQL Docker container.
// (2) - Dapper expects and allows you to write your own SQL.
// (3) - Opens a connection to the ShoppingCart database.
// (4) - Uses a Dapper extension method to execute a SQL query.
// (5) - The result set from the SQL query to ShoppingCartItem.
public class ShoppingCartStore : IShoppingCartStore
{
    // (1)
    private const string ConnectionString =
        @"Data Source=localhost;Initial Catalog=ShoppingCart;
          User Id=SA; Password=Some_password!";

    // (2)
    private const string ReadItemsSql = @"
SELECT ShoppingCart.Id, ProductCatalogId, ProductName, ProductDescription, Currency, Amount
FROM ShoppingCart, ShoppingCartItem
WHERE ShoppingCartItem.ShoppingCartId = ShoppingCart.ID
AND ShoppingCart.UserId = @UserId";

    public async Task<ShoppingCart> Get(int userId)
    {
        await using var conn = new SqlConnection(ConnectionString);     // (3)

        var query = await conn.QueryAsync(
            ReadItemsSql, new { UserId = userId });     // (4)
        var items = query.ToList();

        return new ShoppingCart(
            items.FirstOrDefault()?.ID,
            userId,
            items.Select(x => new ShoppingCartItem(     // (5)
                (int)x.ProductCatalogId,
                x.ProductName,
                x.ProductDescription,
                new Money(x.Currency, x.Amount))));
    }

    //...
}
```

Реализация записи в БД [`ShoppingCartStore`](chapter06/ShoppingCart/ShoppingCart/ShoppingCartStore.cs):

```csharp
// (6) - Create a row in the ShoppingCart table if the shopping cart does not already have an Id.
// (7) - Deletes all pre-existing shopping cart items.
// (8) - Adds the current shopping cart items.
// (9) - Commits all changes to the database.
public class ShoppingCartStore : IShoppingCartStore
{
    // ...

    private const string InsertShoppingCartSql = @"
INSERT INTO ShoppingCart (UserId) OUTPUT inserted.ID VALUES (@UserId)";

    private const string DeleteAllForShoppingCartSql = @"
DELETE item FROM ShoppingCartItem item
INNER JOIN ShoppingCart cart ON item.ShoppingCartId = cart.ID
AND cart.UserId = @UserId";

    private const string AddAllForShoppingCartSql = @"
INSERT INTO ShoppingCartItem(
    ShoppingCartId, ProductCatalogId, ProductName,
    ProductDescription, Amount, Currency)
VALUES(
    @ShoppingCartId, @ProductCatalogId, @ProductName,
    @ProductDescription, @Amount, @Currency)";

    public async Task Save(ShoppingCart shoppingCart)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();
        await using (var tx = conn.BeginTransaction())
        {
            var shoppingCartId = shoppingCart.Id        // (6)
                ?? await conn.QuerySingleAsync<int>(
                    InsertShoppingCartSql, new { shoppingCart.UserId }, tx);

            await conn.ExecuteAsync(                    // (7)
                DeleteAllForShoppingCartSql, new { UserId = shoppingCart.UserId }, tx);

            await conn.ExecuteAsync(                    // (8)
                AddAllForShoppingCartSql,
                shoppingCart.Items.Select(x => new
                {
                    shoppingCartId,
                    x.ProductCatalogId,
                    ProductDescription = x.Description,
                    x.ProductName,
                    x.Price.Amount,
                    x.Price.Currency
                }), tx);

            await tx.CommitAsync();                     // (9)
        }
    }
}
```
