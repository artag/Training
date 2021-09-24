# Lesson 14. Capstone 2

## Советы по написанию приложений

* *Start small*. Не надо заранее проектировать сложный набор объектов и их отношений.
Вместо этого напишите простые функции, и пусть каждая из них делает что-то одно, и делает это
хорошо. Поверьте, что вы сможете скомпоновать их вместе позже.

* *Plug these functions together*. Способы компоновки функций: 
  * Объединить две функции через третью, которая вызывает их обе.
  * Вызывать функцию одну из другой (возможно с помощью функции HOF).

* *Don't be afraid (не бойтесь) of copying and pasting code initially (вначале работы)*.
В F# можно быстро сделать рефакторинг, особенно при использовании higher-order functions.
Когда появится повторяющийся код, тогда и надо его отрефаторить.

* Начинайте писать с путого скрипта `*.fsx`. После успешных экспериментов с кодом в REPL
можно начинать переносить его в `*.fs` файлы.

## Accessing .fs files from a script

Можно загрузить файл `.fs` в скрипт `.fsx`:

```fsharp
#load "Domain.fs"           // Loading .fs files into a script
#load "Operations.fs"
#load "Auditing.fs"

open Capstone2.Operations   // Opening namespaces of .fs files
open Capstone2.Domain
open Capstone2.Auditing
open System
```

Можно использовать `#load` для загрузки как скриптов `.fsx`, так и файлов `.fs`.

Порядок команд `#load` важен; например нельзя загрузить `Operations.fs` до загрузки `Domain.fs`,
если первое зависит от второго.
