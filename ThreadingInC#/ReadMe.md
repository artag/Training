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
