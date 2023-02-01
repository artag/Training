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

### 6.3.3 Storing events raised by a microservice

На рисунке 6.7 показан стандартный набор компонентов в микросервисе; domain model вызывает события. Обычно это происходит, когда происходит изменение или изменения состояния данных, за которые
отвечает микросервис.

![Shopping cart microservice.](images/38_shopping_eventstore.jpg)

Например, в микросервисе корзины покупок, когда пользователь добавляет товар
в свою корзину покупок, вызывается событие Item Added To Shopping Cart, а не RowAddedToShoppingCartTable. Разница в том, что первое означает событие, имеющее значение для системы — пользователь сделал что-то интересное с точки зрения бизнеса, — тогда как второе будет сообщать о технических деталях - часть программного обеспечения выполнила некоторые-
вещь, потому что программист решил реализовать ее таким образом.

События микросервиса должны соответствовать транзакциям бизнес-уровня, а не транзакциям базы данных.
Т.е., например, когда пользователь добавляет товар в свою корзину покупок, вызывается событие
`ItemAddedToShoppingCart`, а не `RowAddedToShoppingCartTable`.
Одно событие микросервиса иногда может включать несколько изменений в БД.

Всякий раз, когда логика домена в микросервисе вызывает событие, оно сохраняется в БД микросервиса.
На рисунке 6.8 это делается с помощью компонента `EventStore`, который отвечает за взаимодействие с базой данных, в которой хранятся события.

![When the domain model raises an event, the EventStore component code must write it to the database.](images/39_save_event_to_db.jpg)

#### Storing events by hand

Шаги по реализации компонента `EventStore`:

1. Add an `EventStore` table to the `ShoppingCart` database. This table will contain
a row for every event raised by the domain model.
2. Use Dapper to implement the writing part of the `EventStore` component.
3. Use Dapper to implement the reading part of the `EventStore` component.

Напоммнание, событие микросервиса `ShoppingCart` выглядит так:

```csharp
public record Event(
    long SequenceNumber, DateTimeOffset OccuredAt, string Name, object Content);
```

Этот тип событий будет сохранятся в отдельной таблице в БД `ShoppingCart`.

Таблица `EventStore`:

| PK | ID        |
|--- |-----------|
| FK | Name      |
|    | OccuredAt |
|    | Content   |

Скрипт для создания таблицы
(полный тут [create-shopping-cart-db.sql](chapter06/ShoppingCart/database-scripts/create-shopping-cart-db.sql)):

```sql
CREATE TABLE dbo.EventStore (
    ID int IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    OccuredAt datetimeoffset NOT NULL,
    Content nvarchar(max) NOT NULL
)
GO
```

Часть реализации класса [EventStore.cs](chapter06/ShoppingCart/EventFeed/EventStore.cs)
для записи событий в БД:

```csharp
public interface IEventStore
{
    // Filtering events based on the start and end points
    Task <IEnumerable<Event>> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber);

    Task Raise(string eventName, object content);
}

// (0) - Uses Dapper to execute a simple SQL insert statement.
public class EventStore : IEventStore
{
    private const string ConnectionString =
        @"Data Source=localhost;Initial Catalog=ShoppingCart;User Id=SA; Password=Some_password!";

    private const string WriteEventSql = @"
INSERT INTO EventStore(Name, OccuredAt, Content)
VALUES (@Name, @OccuredAt, @Content)";

    public async Task Raise(string eventName, object content)
    {
        var jsonContent = JsonSerializer.Serialize(content);
        await using var conn = new SqlConnection(ConnectionString);
        await conn.ExecuteAsync(                    // (0)
            WriteEventSql,
            new
            {
                Name = eventName,
                OccuredAt = DateTimeOffset.Now,
                Content = jsonContent
            });
    }

    // Filtering events based on the start and end points
    public Task<IEnumerable<Event>> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber) =>
        throw new NotImplementedException();
}
```

Cобытие после JSON-сериализации храниться в БД виде строки.

Оставшаяся часть реализации класса [EventStore.cs](chapter06/ShoppingCart/EventFeed/EventStore.cs)
для чтения событий из БД:

```csharp
// (1) - Maps EventStore table rows to Event objects.
// (2) - Reads EventStore table rows between start and end
public class EventStore : IEventStore
{
    private const string ConnectionString =
        @"Data Source=localhost;Initial Catalog=ShoppingCart;User Id=SA; Password=Some_password!";

    private const string ReadEventsSql = @"
SELECT * FROM EventStore
WHERE ID >= @Start AND ID <= @End";

    public async Task<IEnumerable<Event>> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
        await using var conn = new SqlConnection(ConnectionString);
        return await conn.QueryAsync<Event>(            // (1)
            ReadEventsSql,
            new
            {
                Start = firstEventSequenceNumber,       // (2)
                End = lastEventSequenceNumber
            });
    }
}
```

Данная реализация хранилища событий очень проста и не готова для всех случаев использования
в production. Например, когда микросервис начинает вызывать события из нескольких параллельных
потоков, особенно при высокой нагрузке, это может привести к проблемам с блокировкой.

Существует несколько высококачественных проектов с открытым исходным кодом, которые реализуют хранилище событий поверх реляционной базы данных, включая
[SQLStreamStore (https://sqlstreamstore.readthedocs.io)](https://sqlstreamstore.readthedocs.io) и
[Marten https://martendb.io/](https://martendb.io/).

#### Storing events using the EventStoreDB system

Здесь описана другая версия реализации хранения событий в микросервисе shopping cart.
Здесь использется БД `EventStoreDB`. Преимущества использования EventStoreDB:

- API специально предназначен для хранения и чтения событий, а также подписки на новые события.
- У EventStoreDB открытый исходный код
- EventStoreDB может масштабироваться и стабильно работать под нагрузкой.
- Поставляется с некоторыми дополнительными функциями из коробки:
  - Веб-интерфейс для проверки событий.
  - Atom event feeds (каналы событий Atom).

Шаги по реализации:

1. Запуск EventStoreDB в контейнере Docker.
2. Запись событий в EventStoreDB через компонент `EventStore`.
3. Чтение событий из EventStoreDB с помощью компонента `EventStore`.

>Дополнительная информация о EventStoreDB [https://eventstore.com/](https://eventstore.com/).

1. Pull down the latest EventStoreDB docker image:

```text
docker pull eventstore/eventstore
```

2. Запуск контейнера с EventStoreDB:

```text
docker run --name eventstore-node -it -p 2113:2113 -p 1113:1113 --rm eventstore/eventstore:latest --run-projections All --enable-external-tcp --enable-atom-pub-over-http
```

- `--name` - assign a name to the container.
- `-it` - instructs Docker to allocate a pseudo-TTY connected to the container’s stdin;
creating an interactive bash shell in the container.
- `-p` - port forward.
- `--rm` - automatically remove the container when it exits.

>Замечание
>Если показывается сообщение об ошибке
>"TLS is enabled on at least one TCP/HTTP interface - a certificate is required to run EventStoreDB"
>то можно добавить параметр `--insecure` в команду Docker run. Это отключает защиту TLS,
>и не должно использоваться в production.

3. Проверка, запущен ли EventStoreDB

На `http://127.0.0.1:2113/` должно быть приглашение для входа. Логин "admin", пароль "changeit".

#### Добавление кода для работы с EventStoreDB

В проект `ShoppingCart` добавить nuget пакет `EventStore.Client`:

```text
dotnet add package eventstore.client
```

Клиент [`EsEventStore`](chapter06/ShoppingCart/EventFeed/EsEventStore.cs) для EventStoreDb.

Запись событий в БД:

```csharp
namespace ShoppingCart.EventFeed;

public record EventMetadata(DateTimeOffset OccuredAt, string EventName);

public class EsEventStore : IEventStore
{
    private const string ConnectionString =
        "tcp://admin:changeit@localhost:1113";

    // (1) - For local development only.In production, TLS should be enabled.
    // (2) - Creates a connection to EventStore.
    // (3) - Opens the connection to EventStore.
    // (4) - Writes the event to EventStore.
    // (5) - EventData is EventStore's representation of an event.
    // (6) - Maps OccurredAt and EventName to metadata to be stored along with the event.
    public async Task Raise(string eventName, object content)
    {
        var settings = ConnectionSettings.Create().DisableTls().Build();            // (1)
        var uri = new Uri(ConnectionString);
        using var connection = EventStoreConnection.Create(settings, uri);          // (2)

        await connection.ConnectAsync();                                            // (3)

        var jsonContent = JsonSerializer.Serialize(content);
        var data = Encoding.UTF8.GetBytes(jsonContent);

        var eventMetadata = new EventMetadata(DateTimeOffset.UtcNow, eventName);    // (6)
        var jsonMetadata = JsonSerializer.Serialize(eventMetadata);
        var metadata = Encoding.UTF8.GetBytes(jsonMetadata);

        var events = new[]
        {
            new EventData(                                  // (5)
                eventId: Guid.NewGuid(),
                type: "ShoppingCartEvent",
                isJson: true,
                data: data,
                metadata: metadata)
        };

        await connection.ConditionalAppendToStreamAsync(    // (4)
            stream: "ShoppingCart",
            expectedVersion: ExpectedVersion.Any,
            events: events);
    }

    // ... Чтение событий из БД.
}
```

Чтение событий из БД (см. [`EsEventStore`](chapter06/ShoppingCart/EventFeed/EsEventStore.cs)):

```csharp
namespace ShoppingCart.EventFeed;

public record EventMetadata(DateTimeOffset OccuredAt, string EventName);

public class EsEventStore : IEventStore
{
    private const string ConnectionString =
        "tcp://admin:changeit@localhost:1113";

    // ... Запись событий в БД.

    // (7) - Reads events from the EventStoreDB.
    // (8) - Accesses the events on the result from the EventStoreDB.
    // (9) - Gets the content part of each event.
    // (10) - Gets the metadata part of each event.
    // (11) - Maps to events from EventStoreDB Event objects.
    public async Task<IEnumerable<Event>> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
        var settings = ConnectionSettings.Create().DisableTls().Build();
        var uri = new Uri(ConnectionString);
        using var connection = EventStoreConnection.Create(settings, uri);

        await connection.ConnectAsync();

        var count = (int)(lastEventSequenceNumber - firstEventSequenceNumber);
        var result = await connection.ReadStreamEventsForwardAsync(     // (7)
            stream: "ShoppingCart",
            start: firstEventSequenceNumber,
            count: count,
            resolveLinkTos: false);

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        return result.Events        // (8)
            .Select(e =>
                new
                {
                    Content = Encoding.UTF8.GetString(e.Event.Data),            // (9)
                    Metadata = JsonSerializer.Deserialize<EventMetadata>(       // (10)
                        Encoding.UTF8.GetString(e.Event.Metadata), options)
                })
            .Select((e, i) =>       // (11)
                new Event(
                    SequenceNumber: i + firstEventSequenceNumber,
                    OccuredAt: e.Metadata.OccuredAt,
                    Name: e.Metadata.EventName,
                    Content: e.Content));
    }
}
```

### 6.3.4 Setting cache headers in HTTP responses

Еще раз рассмотрим рис. 6.2:

![Partitioning data between microservices.](images/34_partitioning_data.jpg)

Микросервис `ShoppingCart` запрашивает у `ProductCatalog` данные по endpoint'у `/products`.

#### Проект `ProductCatalog`

По сути, в данной главе реализованы только пара классов: `ProductCatalogController` и
`ProductStore`.

Создание проекта как было ранее в примерах (например, см. [главу 2](Chapter02.md)):

```text
dotnet new web -n ProductCatalog
```

Endpoint `/products` реализуется при помощи [`ProductCatalogController`](chapter06/ProductCatalog/ProductCatalogController.cs).
В запросе указываются id продуктов, разделенные запятыми.

Реализация [`ProductCatalogController`](chapter06/ProductCatalog/ProductCatalogController.cs):

```csharp
namespace ProductCatalog;

[Route("/products")]
public class ProductCatalogController : ControllerBase
{
    private readonly IProductStore _productStore;

    public ProductCatalogController(IProductStore productStore)
    {
        _productStore = productStore;
    }

    // (1) - Adds a cache-control header, with max-age in seconds
    [HttpGet("")]
    [ResponseCache(Duration = 86400)]    // (1)
    public IEnumerable<ProductCatalogProduct> Get([FromQuery] string productIds)
    {
        var products = _productStore.GetProductsByIds(
            ParseProductIdsFromQueryString(productIds));
        return products;
    }

    private static IEnumerable<int> ParseProductIdsFromQueryString(string productIdsString) =>
        productIdsString
            .Split(',')
            .Select(s => s.Replace("[", "").Replace("]", ""))
            .Select(int.Parse);
}
```

Атрибут `ResponseCache` добавляет header `cache-control` в ответы и разрешает кэширование
в течение 86400 секунд (24 часа). Заголовок ответа будет выглядеть так:

```csharp
cache-control: public,max-age:86400
```

В книге не показана реализация [`IProductStore`](chapter06/ProductCatalog/ProductStore.cs).
Такая псевдо-реализация хранилища в данном микросервисе.

```csharp
namespace ProductCatalog;

public record Money();
public record ProductCatalogProduct(
    int ProductId, string ProductName, string Description, Money Price);

public interface IProductStore
{
    IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds);
}

public class ProductStore : IProductStore
{
    public IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds) =>
        productIds.Select(id => new ProductCatalogProduct(id, "foo" + id, "bar", new Money()));
}

```

### 6.3.5 Reading and using cache headers

В [главе 2](Chapter02.md) в `ShoppingCart` был создан `ProductCatalogClient` для запросов
endpoint'а `/products` у микросервиса `ProductCatalog`.

В этой главе он был модернизирован
[`ProductCatalogClient.cs`](chapter06/ShoppingCart/ProductCatalogClient.cs):

```csharp
namespace ShoppingCart;

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
}

public class ProductCatalogClient : IProductCatalogClient
{
    // URL of the fake product catalog microservice
    private static readonly string ProductCatalogBaseUrl = @"https://git.io/JeHiE";
    private static readonly string GetProductPathTemplate = "?productIds=[{0}]";

    private readonly HttpClient _client;
    private readonly ICache _cache;

    // (1) Configure the HttpClient to use the base address of the product catalog.
    // (2) Configure the HttpClient to accept JSON responses from the product catalog.
    public ProductCatalogClient(HttpClient client, ICache cache)
    {
        client.BaseAddress = new Uri(ProductCatalogBaseUrl);    // (1)
        client.DefaultRequestHeaders.Accept                     // (2)
            .Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        _client = client;
        _cache = cache;
    }

    // Fetching products and converting them to shopping cart items
    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(
        int[] productCatalogIds)
    {
        using var response = await RequestProductFromProductCatalog(productCatalogIds);
        return await ConvertToShoppingCartItems(response);
    }

    // (3) Tries to retrieve a valid response from the cache.
    // (4) Only makes the HTTP request if there’s no response in the cache.
    // (5) Tells HttpClient to perform the HTTP GET asynchronously.
    private async Task<HttpResponseMessage> RequestProductFromProductCatalog(
        int[] productCatalogIds)
    {
        var productsResource = string.Format(
            GetProductPathTemplate,
            string.Join(",", productCatalogIds));
        var response = _cache.Get(productsResource) as HttpResponseMessage;     // (3)
        if (response is null)                                                   // (4)
        {
            response = await _client.GetAsync(productsResource);                // (5)
            AddToCache(productsResource, response);
        }

        return response;
    }

    // (6) Reads the cache-control header from the response.
    // (7) Parses the cache-control value and extracts max-age from it.
    // (8) Adds the response to the cache if it has a max-age value.
    private void AddToCache(string resource, HttpResponseMessage response)
    {
        var cacheHeader = response.Headers
            .FirstOrDefault(h => h.Key == "cache-control");                 // (6)
        if (!string.IsNullOrEmpty(cacheHeader.Key)
            && CacheControlHeaderValue.TryParse(                            // (7)
                cacheHeader.Value.ToString(), out var cacheControl)
            && cacheControl?.MaxAge.HasValue is true)
            _cache.Add(resource, response, cacheControl.MaxAge!.Value);     // (8)
    }

    // (9) Uses System.Text.Json to deserialize the JSON from the product catalog microservice.
    // (10) Creates a ShoppingCartItem for each product in the response.
    // (11) Uses a private record to represent the product data.
    private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(
        HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var products =
            await JsonSerializer.DeserializeAsync<List<ProductCatalogProduct>>(stream, options)
            ?? new List<ProductCatalogProduct>();   // (9)

        return products.Select(p =>
            new ShoppingCartItem(                   // (10)
                p.ProductId,
                p.ProductName,
                p.ProductDescription,
                p.Price));
    }

    private record ProductCatalogProduct(           // (11)
        int ProductId, string ProductName, string ProductDescription, Money Price);
}
```

Здесь появилась ссылка на использование кэша `ICache`. Теперь каждый раз при запросе данных
у сервиса `ProductCatalog` все ответы будут кэшироваться и будут считаться действительными
в течение заданного значения `max-age` в header'е `cache-control`.
В этом примере значение `max-age` составляет 86400 секунд (24 часа).

Вот тут идет обращение к кэшу перед вызовом удаленного `ProductCatalog`:

```csharp
// (3) Tries to retrieve a valid response from the cache.
// (4) Only makes the HTTP request if there’s no response in the cache.
// (5) Tells HttpClient to perform the HTTP GET asynchronously.
private async Task<HttpResponseMessage> RequestProductFromProductCatalog(
    int[] productCatalogIds)
{
    var productsResource = string.Format(
        GetProductPathTemplate,
        string.Join(",", productCatalogIds));
    var response = _cache.Get(productsResource) as HttpResponseMessage;     // (3)
    if (response is null)                                                   // (4)
    {
        response = await _client.GetAsync(productsResource);                // (5)
        AddToCache(productsResource, response);
    }

    return response;
}

// (6) Reads the cache-control header from the response.
// (7) Parses the cache-control value and extracts max-age from it.
// (8) Adds the response to the cache if it has a max-age value.
private void AddToCache(string resource, HttpResponseMessage response)
{
    var cacheHeader = response.Headers
        .FirstOrDefault(h => h.Key == "cache-control");                 // (6)
    if (!string.IsNullOrEmpty(cacheHeader.Key)
        && CacheControlHeaderValue.TryParse(                            // (7)
            cacheHeader.Value.ToString(), out var cacheControl)
        && cacheControl?.MaxAge.HasValue is true)
        _cache.Add(resource, response, cacheControl.MaxAge!.Value);     // (8)
}
```

Реализация `ICache` в книге не показана. Лежит тут [`Cache.cs`](chapter06/ShoppingCart/Cache.cs):

```csharp
namespace ShoppingCart;

public interface ICache
{
    // ttl means time to live.
    void Add(string key, object value, TimeSpan ttl);
    object? Get(string key);
}

public class Cache : ICache
{
    private readonly IDictionary<string, (DateTimeOffset, object)> _cache =
        new ConcurrentDictionary<string, (DateTimeOffset, object)>();

    public void Add(string key, object value, TimeSpan ttl) =>
        _cache[key] = (DateTimeOffset.UtcNow.Add(ttl), value);

    public object? Get(string productsResource)
    {
        if (_cache.TryGetValue(productsResource, out var value)
            && value.Item1 > DateTimeOffset.UtcNow)
            return value;

        _cache.Remove(productsResource);
        return null;
    }
}
```

## Запуск в Docker Compose

*(В книге не упоминается)*

Файл [`docker-compose.yaml`](chapter06/docker-compose.yaml) для запуска БД в Docker Compose:

```yaml
# Файл docker-compose должен начинаться с тега версии.
version: '3.4'

# docker-compose работает с сервисами.
# 1 сервис = 1 контейнер.
# Раздел, в котором будут описаны сервисы, начинается с 'services'.
services:
  # Первый сервис (контейнер).
  # Назвать его можно так, как нужно разработчику.
  eventstore.db:
    image: eventstore/eventstore:latest
    environment:
      - EVENTSTORE_CLUSTER_SIZE=1
      - EVENTSTORE_RUN_PROJECTIONS=All
      - EVENTSTORE_START_STANDARD_PROJECTIONS=true
      - EVENTSTORE_EXT_TCP_PORT=1113
      - EVENTSTORE_HTTP_PORT=2113           # в книге EVENTSTORE_EXT_HTTP_PORT=2113
      - EVENTSTORE_INSECURE=true
      - EVENTSTORE_ENABLE_EXTERNAL_TCP=true
      - EVENTSTORE_ENABLE_ATOM_PUB_OVER_HTTP=true
    # [порт компьютера]:[порт контейнера]
    ports:
      - "1113:1113"
      - "2113:2113"

  # Второй сервис (контейнер).
  sqlserver.db:
    image: mcr.microsoft.com/mssql/server
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Some_password!
    # [порт компьютера]:[порт контейнера]
    ports:
      - 1433:1433
```

Команда                                        | Описание
-----------------------------------------------|------------------------------------
`docker-compose build`                         | Сборка проекта
`docker-compose up`                            | Запускает проект
`docker-compose down`                          | Останавливает и удаляет контейнеры и другие ресурсы
`docker-compose logs -f [service name]`        | Выводит журналы сервисов
`docker-compose ps`                            | Выводит список контейнеров
`docker-compose exec [service name] [command]` | Выполняет команду в выполняющемся контейнере
`docker-compose images`                        | Выводит список образов
`docker-compose rm`                            | Removes stopped service containers

Примеры некоторых команд:

```text
docker-compose logs -f eventstore.db
docker-compose exec sqlserver.db ls
```

Для `docker-compose up` ключи:

`-d`, `--detach` - Detached mode: Run containers in the background

## Summary

- Микросервис хранит и владеет всеми данными, относящимися к функциям, реализуемым этим
микросервисом.

- Микросервис является authoritative (авторитетным) источником данных, которыми он владеет.

- Микросервис хранит свои данные в собственной отдельной базе данных.

- Микросервис часто кэширует данные, принадлежащие другим микросервисам, по следующим
причинам:

  - Чтобы уменьшить coupling (связносность/связь) с другими микросервисами.
  Это делает работу всей системы более стабильной.

  - Ускорение работы, при уменьшении частоты удаленных вызовов.

  - Для создания собственных пользовательских представлений (известны как read models - данные
  принадлежащие другому микросервису).

  - Создавать read models на основе событий из других микросервисов, чтобы избежать использования
  запросов к другим микросервисам. В [главе 5](Chapter05.md) было сказано, что взаимодействие
  между микросервисами на основе событий предпочтительнее из-за уменьшения coupling.

- Разные микросервисы могут использовать разные типы баз данных.

- Хранение данных, принадлежащих микросервису, аналогично хранению данных в других
видах/типах систем.

- Можно использовать Dapper для чтения и записи данных в базу данных SQL .

- Хранение событий - это, по сути, сохранение сериализованного события в базе данных.

- Простая версия хранилища событий - это хранение событий в таблице в базе данных SQL.

- Для хранения событий можно использовать EventStoreDB, который специально для этого разработан.
