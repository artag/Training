# Threading in C#

- [001_NewThread](001_NewThread/ReadMe.md)

Два thread (методы) параллельно печатают в консоль.

- [002_NewThreadWithLocalVar](002_NewThreadWithLocalVar/ReadMe.md)

Метод запущенный из thread использует локальную переменную.

- [003_NewThreadWithSameObject](003_NewThreadWithSameObject/ReadMe.md)

Один экземпляр класса запущенный из двух thread. Меняется локальная переменная класса.

- [004_NewThreadWithSameStaticField](004_NewThreadWithSameStaticField)

Два thread делят общую статическую переменную. Недерменированное поведение.

- [005_NewThreadWithLock](005_NewThreadWithLock/ReadMe.md)

Использование `lock` как средство синхронизации и параллельного доступа к общей статической переменной
(переделанный предыдущий пример).

- [006_ThreadJoin](006_ThreadJoin/ReadMe.md)

Thread `Main` ждет завершения другого thead через использование `t.Join()`.

- [007_ThreadWithDelegateOrLambda](007_ThreadWithDelegateOrLambda/ReadMe.md)

Запуск в thread делегата `ThreadStart` и lambda функции.

- [008_PassingDataToThread](008_PassingDataToThread/ReadMe.md)

Различные способы передачи данных в метод, запущенный в thread:
через делегат, через lambda функцию, через параметр `object`.

- [009_PassingClosureToThread](009_PassingClosureToThread/ReadMe.md)

Использование замыканий при передаче параметра в метод, запущенный в thread.

- [010_NamingThreads](010_NamingThreads/ReadMe.md)

Присваивание имен thread'ам (Main и второму). Получение (и вывод в консоль) имен обоих thread.

- [011_BackgroundThread](011_BackgroundThread/ReadMe.md)

Запуск thread в `Background` режиме.

- [012_NoExceptionHandlingInThread](012_NoExceptionHandlingInThread/ReadMe.md)

Пример выброса необработанного исключения из thread.

- [013_ExceptionHandlingInThread](013_ExceptionHandlingInThread/ReadMe.md)

Обработка исключения в thread.

- [014_TaskFactoryStartNew](014_TaskFactoryStartNew/ReadMe.md)

Создание и запуск task через `Task.Factory.StartNew`.
Ожидание завершения работы task через `t.Wait()`.
Обработка выброшенного исключения из task через `AggregateException`.

- [015_StartTaskWithReturnValue](015_StartTaskWithReturnValue/ReadMe.md)

Создание и запуск task через `Task.Factory.StartNew` с передачей параметра в метод.
Возврат результата после завершения выполнения task - через `task.Result`.

- [016_QueuedWorkItem](016_QueuedWorkItem/ReadMe.md)

Использование `ThreadPool.QueueUserWorkItem`.
Запуск метода в потоке из пула потоков без параметров, с передачей параметра через `object`,
передача типизированного парамера, обработка исключения внутри потока.

- [017_AsynchronousDelegate](017_AsynchronousDelegate/ReadMe.md)

`BeginInvoke`, `EndInvoke`. На .NET 8 не работают.

- [018_ThreadUnsafe](018_ThreadUnsafe/ReadMe.md)

Попытка продемонстрировать проблемы синхронизации общего статического поля при его изменении
из разных thread.

- [019_ThreadSafeWithLock](019_ThreadSafeWithLock)

Синхронизация общего статического поля при его изменении из разных thread.
Использование `lock`.

- [020_ThreadSafeWithMonitorEnterExit](020_ThreadSafeWithMonitorEnterExit/ReadMe.md)

Синхронизация общего статического поля при его изменении из разных thread.
Использование `Monitor.Enter`, `Monitor.Exit`.
По сути, `lock` заменяет эти конструкции.

- [021_MonitorTryEnter](021_MonitorTryEnter/ReadMe.md)

Использование `Monitor.TryEnter`, `Monitor.Exit`.
Попытка получить блокировку в течение `timeout` (в `Monitor.TryEnter`).
Если `timeout` не задан, то ожидания получения блокировки нет.

- [022_NestedLocking](022_NestedLocking/ReadMe.md)

Демонстрация работы вложенных `lock`.

- [023_OneProcessOnlyWithMutex](023_OneProcessOnlyWithMutex/ReadMe.md)

Использование `Mutex` и `mutex.WaitOne()`.
**Реализация** запуска только одного экземпляра приложения на локальной машине.
При одном работающем экземпляре другой не запустится.
`Mutex` в 50 раз медленнее чем `lock`.

- [024_SemaphoreSlimExample](024_SemaphoreSlimExample/ReadMe.md)

Пример использования `SemaphoreSlim` (и `Wait` и `Release`).

**Аналогия**: использование этого семафора похоже на ночной клуб с вышибалой.
Клуб имеет ограниченную вместимость, а толпа на улице пытается попасть в клуб.
Вышибала не пускает в клуб больше положенного, поэтому
лишний народ ожидает на улице, пока кто-нибудь не выйдет из клуба. Как только один человек выходит
из клуба, другой тут же заходит в клуб.

Семафоры могут быть полезны для ограничения одновременно запущенных потоков.

- [025_ListWithLock](025_ListWithLock/ReadMe.md)

Потокобезопасный доступ к `List` из разных потоков.
Операции добавления и копирования элементов в `List` через `lock`.
Чтение из `List` потокобезопасное. (Даже при чтении через enumeration).

- [026_UserCacheThreadSafe](026_UserCacheThreadSafe/ReadMe.md)

Пример **реализации** потокобезопасного кэша (через `Dictionary` и `lock`).

Здесь потокобезопасная реализация, но при начале работы кэш обновляется два раза из-за одновременного
его вызова из разных потоков.
Это компромисс между простотой и скоростью работы кэша.

- [027_AutoResetEventExample](027_AutoResetEventExample/ReadMe.md)

Пример использования `AutoResetEvent`.

**Аналогия**: `AutoResetEvent` работает как билетный турникет: он пропускает только по одному
человеку после предъявления им билета. После прохода человека турникет закрывается до
предъявления следующего билета.

Thread блокируется и ожидает прихода сигнала, после вызова метода `waitHandle.WaitOne()`.

Другой объект/поток через `AutoResetEvent` "показывает билет" путем вызова метода `waitHandle.Set()`
и разблокирует ожидающий поток.

- [028_AutoResetEventTwoWaySignaling](028_AutoResetEventTwoWaySignaling/ReadMe.md)

Пример **использования** двух `AutoResetEvent` для Two-way signaling.
Два thread управляют друг-другом: один ожидает, другой его запускает и наоборот.

- [029_AutoResetEventProducerConsumerQueue](029_AutoResetEventProducerConsumerQueue/ReadMe.md)

Пример **реализации** Producer-Consumer через `AutoResetEvent`.

Подобый функционал можно также реализовать через `BlockingCollection<T>`.

- [030_ManualResetEventExample](030_ManualResetEventExample/ReadMe.md)

**Аналогия**: `ManualResetEvent` работает как обычные ворота.
Один или несколько threads ожидают (метод `WaitOne`) прохода через ворота.
`Set` открывает ворота. `Reset` закрывает ворота.

**Сценарий**: 1 thread запускает выполнение N thread'ов.

- [031_CountdownEventExample](031_CountdownEventExample/ReadMe.md)

**Сценарий**: N threads запускают выполнение 1 thread'а.

Пример использования `CountdownEvent`.

- [032_WaitHandlesThreadPool](032_WaitHandlesThreadPool/ReadMe.md)

Пример использования `ThreadPool.RegisterWaitForSingleObject`.
Этот метод принимает делегат, который вызывается после сигнала wait handle (вызов метода `Set`).

**Применение**: если приложение имеет множество threads, которые тратят много времени на ожидание
wait handle.

- [033_ContinueWithoutWaiting](033_ContinueWithoutWaiting/ReadMe.md)

Невнятный пример.

- [034_WaitAny](034_WaitAny/ReadMe.md)

Ожидание до завершения первого thread из нескольких.

Использование `WaitHandle.WaitAny()` и нескольких `AutoResetEvent`.

- [035_WaitAll](035_WaitAll/ReadMe.md)

Ожидание завершения всех threads.

Использование `WaitHandle.WaitAll()` и нескольких `AutoResetEvent`.

- [036_SignalAndWait](036_SignalAndWait/ReadMe.md)

Толком непонятно, что делает `WaitHandle.SignalAndWait`.

- [037_WebClientAsEAP](037_WebClientAsEAP/ReadMe.md)

Пример использования `WebClient` (устарел) как event-based asynchronous pattern (EAP).
Здесь `WebClient` скачивает в фоне строку через метод `DownloadStringAsync`.
По завершении скачивания вызывается событие `wc.DownloadStringCompleted`.

Здесь клиенту не требуется явно вызывать или управлять thread'ами.

Task'и поддерживают похожую функциональность, поэтому сейчас EAP применяется мало.

- [038_BackgroundWorkerBasics](038_BackgroundWorkerBasics/ReadMe.md)

Использование `BackgroundWorker` как основу для реализации EAP.
Использует thread pool.

Инструкция по применению.

1) Создать экземпляр `BackgroundWorker`.

2) Определить обработчик события `DoWork` - выполнение кода в фоновом потоке.

3) Определить обработчик события `RunWorkerCompleted` - выполенение кода после завершения
фонового потока.

- [039_BackgroundWorkerAdvanced](039_BackgroundWorkerAdvanced/ReadMe.md)

Продвинутое использование `BackgroundWorker`. Возможности:

0) Возможности из предыдущего примера, плюс

1) Возможность прервать работу `BackgroundWorker` (через `CancelAsync()` и т.д.)

2) Отслеживание прогресса выполнения (через `ReportProgress()` и `ProgressChanged` и т.д.)

3) Передача результата из фонового потока в последующий код (через `DoWorkEventArgs` и т.д.)

- [040_BackgroundWorkerSubclassing](040_BackgroundWorkerSubclassing/ReadMe.md)

Пример реализации класса на основе `BackgroundWorker` (subclassing, наследник).

**Применение**: требуется реализовать только один асинхронный метод в классе через EAP.

- [041_InterruptThread](041_InterruptThread/ReadMe.md)

`Interrupt` для thread используется редко, `Abort` используется чаще.

На **заблокрованном** thread вызов `Interrupt()` рпинудительно освобождает его, кидается
`ThreadInterruptedException`.

`Abort` на .NET Core имеет статус **obsolete** и кидает `PlatformNotSupportedException`.

Рекомендуется не использовать `Interrupt`, а пользоваться либо signaling construct, либо
cancellation tokens.

- [042_SafeCancellationImplementing](042_SafeCancellationImplementing/ReadMe.md)

Пример реализации концепции `CancellationToken` "руками".

Исключение `OperationCanceledException` используется именно для таких целей. Правда ничто не мешало
сделать реализацию safe cancellation, используя другой вид исключения.

- [043_UsingCancellationToken](043_UsingCancellationToken/ReadMe.md)

Пример использования `CancellationTokenSource` и `CancellationToken`.

- [044_LazyInitializationImplementing](044_LazyInitializationImplementing/ReadMe.md)

Пример **Lazy Initialization**.

Используется `lock` внутри свойства для вызова конструктора. Объект создается только раз.
Thread-safe вызов.

Рекомендуется использовать `Lazy<T>` (подход из 045 примера).

- [045_LazyThreadSafeUsing](045_LazyThreadSafeUsing/ReadMe.md)

Пример **Lazy Initialization**.

Используется `Lazy<T>` для вызова конструктора. Объект создается только раз.
Thread-safe вызов.

Более эффективен, чем в случае примера 044, т.к. использует double-checked locking.

- [046_LazyInitializerUsing](046_LazyInitializerUsing/ReadMe.md)

Пример **Lazy Initialization**.

Пример использования `LazyInitializer`. Более быстрый по сравнению `Lazy`, но код менее читаем.
Если добавить дополнительный аргумент `lock`, то можно использовать в multi-core окружении.

- [047_DoubleCheckedLocking](047_DoubleCheckedLocking/ReadMe.md)

Пример **Lazy Initialization**.

Аналог `LazyInitializer` - ручная реализация похожего поведения через double-checked locking
Используется `lock`.
Работает также, как и предыдущий пример.

- [048_RaceToInitializePattern](048_RaceToInitializePattern/ReadMe.md)

Пример **Lazy Initialization**.

Аналог `LazyInitializer` - ручная реализация похожего поведения через race-to-initialize pattern.
Используется `Interlocked.CompareExchange` и `volatile` переменная класса.

Работает также, как и предыдущий пример.

- [049_ThreadStatic](049_ThreadStatic/ReadMe.md)

Пример **Thread local storage**.

Каждый thread имеет доступ к глобальной переменной. Каждый thread при взаимодействии с этой переменной
получает ее копию. Т.е. изменения данной переменной не видны в других thread'ах.

Способ 1 из 3. Пометка глобальной статической переменной
при помощи атрибута `[ThreadStatic]`

- [050_ThreadLocal](050_ThreadLocal/ReadMe.md)

Пример **Thread local storage**.

Способ 2 из 3. Создание thread-local storage для статической/класса переменной.
Можно задать начальное значение переменной.
Используется обертка `ThreadLocal<T>` вокруг этой переменной.

- [051_GetDataAndSetData](051_GetDataAndSetData/ReadMe.md)

Пример **Thread local storage**.

Способ 3 из 3. Использование `LocalDataStoreSlot`. Через `Thread.GetData(_slot)` можно прочитать
значение из определенного слота, через `Thread.SetData(_slot, value)` записать значение в слот.
