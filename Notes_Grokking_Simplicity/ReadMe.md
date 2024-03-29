# Grokking Simplicity

* Part 1: Actions, calculations, and data
  * [Chapters 01 - 05](Part1_1.md)
    * 01 Welcome to Grokking Simplicity
    * 02 Functional thinking in action
    * 03 Distinguishing actions, calculations, and data
    * 04 Extracting calculations from actions
    * 05 Improving the design of actions
  * [Chapters 06 - 09](Part1_2.md)
    * 06 Staying immutable in a mutable language
    * 07 Staying immutable with untrusted code
    * 08 Stratified design: Part 1
    * 09 Stratified design: Part 2

* Part 2: First-class abstractions

  * [Chapters: 10-13](Part2_1.md)
    * 10 First-class functions: Part 1
    * 11 First-class functions: Part 2
    * 12 Functional iteration
    * 13 Chaining functional tools
  * [Chapters: 14-16](Part2_2.md)
    * 14 Functional tools for nested data
    * 15 Isolating timelines
    * 16 Sharing resources between timelines
  * [Chapters: 17-19](Part2_3.md)
    * 17 Coordinating timelines
    * 18 Reactive and onion architectures
    * 19 The functional journey ahead

## Chapter 1 and 2

### Основные действия при рефакторинге кода. Что изучается в книге

* Distinguishing actions, calculations, and data.
* Organizing code by "rate of change".
(A first glimpse of stratified design - первый взгляд на многослойный дизайн).
* First-class abstractions.
* Timelines visualize distributed systems.
  * Multiple timelines can execute in different orderings.

### Hard-won lessons about distributed systems

1. Timelines are uncoordinated by default.
2. You cannot rely on (полагаться) the duration of actions.
3. Bad timing, however rare, can happen in production.
4. The timeline diagram reveals (показывают) problems in the system.

## Chapter 3. Distinguishing actions, calculations, and data

1. Actions, calculations, and data can be apply to any situation.
2. Actions can hide actions, calculations, and data.
3. Calculations can be composed of smaller calculations and data.
4. Data can only be composed of more data.
5. Calculations often happen "in our heads".
6. Actions spread (распространяются) through code.
7. Actions can take many forms
   * Function calls
   * Method calls
   * Constructors
   * Expressions
   * Statements

### Data

Data is facts about events. It is a record of something that happened.

Unlike actions and calculations, it cannot be run.

#### Immutability

Functional programmers use two main disciplines for implementing immutable data:

1. *Copy-on-write*. Make a copy of data before you modify it.
2. *Defensive copying*. Make a copy of data you want to keep.

See chapters 6 and 7.

#### Advantages and disadvantages

+ Serializable. (Хранение, передача)

+ Compare for equality. (Возможность сравнения)

+ Open for interpretation.

- Data must be interpreted to be useful.

### Calculations

Calculations are computations from inputs to outputs. No matter when they are run, or how
many times they are run, they will give the same output for the same inputs.

#### Why prefer calculations to actions?

Calculations have benefits compared to actions:

* They're much easier to test.
* They're easier to analyze by a machine.
* They're very composable.
* Much easier to understand:
  * What else is running at the same time
  * What has run in the past and what will run in the future
  * How many times you have already run it.

### Actions

Actions are anything that have an effect on the world or are affected by the world. As a
rule of thumb, actions depend on when or how many times they are run.

* When they are run - *Ordering*
* How many times they are run - *Repetition*

#### Советы по использованию actions

1. Use fewer actions if possible.
2. Keep your actions small. Remove everything that isn't necessary from the
action.
3. Restrict (ограничьте) your actions to interactions with the outside.
4. Limit how dependent on time an action is.

## Chapter 4. Extracting calculations from actions

* Extract calculations from actions
* Copy-on-write
* Aligning design with business requirements
  * Choosing a better level of abstraction that matches usage

*Explicit inputs* - Arguments.

*Implicit inputs* - Any other input

*Explicit outputs* - Return value

*Implicit outputs* - Any other output

Copying a mutable value before you modify it is a way to implement immutability.
It's called *copy-on-write*. (See chapter 6).

### Step-by-step: Extracting a calculation

**1. Select and extract the calculation code**

Select a suitable chunk of code for extraction. Refactor that chunk into a new function.
You may have to add arguments where appropriate. Make sure to call the new function
where the chunk was in the old function.

**2. Identify the implicit inputs and outputs of the function**

Identify the inputs and outputs of the new function. The inputs are anything that can
affect the result between calls to the function. And the outputs are anything that can be
affected by calling that function.
Example inputs include arguments, reads from variables outside the function, queries
of a database, and so on.
Example outputs include return value, modifying a global variable, modifying a shared
object, sending a web request, and so on.

**3. Convert inputs to arguments and outputs to return values**

One at a time, convert inputs to arguments and outputs to return values. If you add
return values, you may need to assign that value to a local variable in the original
function.

Important. If we want our arguments and return values to be
immutable values - that is, they don't change. If we return a value and some piece of our
function later changes it, that's a kind of implicit output. Similarly, if something changes
the argument values after our function has received them, that is a kind of implicit input.

## Chapter 5. Improving the design of actions

A *code smell* is a characteristic of a piece of code that might be a symptom of deeper problems.

### Principles

* Minimize (reducing) implicit inputs and outputs.

* Giving the code a once-over (беглый осмотр).

* Categorizing our calculations.
  * By grouping our calculations (layers of meaning)

* Smaller functions and more calculations.

* Design is about pulling things apart (разделение сущностей на части).
  * Easier to reuse.
  (Smaller, simpler functions are easier to reuse. They do less.
They make fewer assumptions.)

  * Easier to maintain.
  (Smaller functions are easier to understand and maintain. They have less code.
They are often obviously right (or wrong)).

  * Easier to test.
  (Smaller functions are easier to test. They do one thing, so you just test that
one thing).

## Chapter 6. Staying immutable in a mutable language

### Categorizing operations into reads, writes, or both

* **Reads**
  * Get information out of data
  * Do not modify the data
* **Writes**
  * Modify the data

### The three steps of the copy-on-write discipline

1. Make a copy.
2. Modify the copy (as much as you want!).
3. Return the copy.

* Copy-on-write converts writes into reads.

* These copy-on-write operations are generalizable

### What to do if an operation is a read and a write

Two approaches:

1. Split function (Splitting the operation into read and write).
2. Return two values.

*Пример*.

Remove from the front `.shift` This mutates the array by dropping the first element
(index 0) and returns the value that was dropped.

```js
> var array = [1, 2, 3, 4];
> array.shift()
1
> array
[2, 3, 4]
```

*Подход 1*. Splitting the operation into read and write:

```js
// just a function that returns the first element (or undefined if the list is empty).
// it's a calculation
function first_element(array) {
    return array[0];
}

// perform the shift but drop the return value
// Copy-on-write
function drop_first(array) {
    var array_copy = array.slice();
    array_copy.shift();
    return array_copy;
}
```

*Подход 2*. Returning two values from one function:

```js
function shift(array) {
    var array_copy = array.slice();
    var first = array_copy.shift();
    return {    // we use an object to return two separate values
        first : first,
        array : array_copy
    };
}
```

### Reads to immutable data structures are calculations

* Reads to mutable data are actions.
* Writes make a given piece of data mutable.
* If there are no writes to a piece of data, it is immutable.
* Reads to immutable data structures are calculations.
* Converting writes to reads makes more code calculations.

### Immutable data structures are fast enough

* We can always optimize later.
* Garbage collectors are really fast.
* We're not copying as much as you might think at first.
  * shallow copy (just copy the top level of a data structure)
  * structural sharing (copies share a lot of references to the same objects in memory)
* Functional programming languages have fast implementations.

### Copy-on-write operations on objects

1. Make a copy.
2. Modify the copy.
3. Return the copy.

## Chapter 7. Staying immutable with untrusted code

Any data that leaves the safe zone is potentially mutable. Likewise,
any data that enters the safe zone from untrusted code
is potentially mutable.

We can use *defensive copies* to protect data and maintain immutability.

### Rule 1: Copy as data leaves your code

1. Make a deep copy of the immutable data.
2. Pass the copy to the untrusted code.

<img src="images/ch07_data_leaves.jpg" alt="Data leaves the safe zone"/>

### Rule 2: Copy as data enters your code

1. Immediately make a deep copy of the mutable data passed to your code.
2. Use the copy in your code.

<img src="images/ch07_data_enters.jpg" alt="Data enters the safe zone from the untrusted code"/>

## Chapter 8. Stratified design: Part 1

*Stratified design* is a design technique that builds software in layers.

Each layer defines new functions in terms of the functions in the layers below it.

Пример:

<img src="images/ch08_stratified_design.jpg" alt="Stratified design"/>

### Характеристики, которые могут использоваться в качестве критериев для дизайна

**Function bodies**

* Length
* Complexity
* Levels of detail
* Functions called
* Language features used

**Layer structure**

* Arrow length
* Cohesion
* Level of detail

**Function signatures**

* Function name
* Argument names
* Argument values
* Return value

### Решения, принимаемые при проектировании дизайна

**Organization**

* Decide where a new function goes.
* Move functions around.

**Implementation**

* Change an implementation.
* Extract a function.
* Change a data structure.

**Changes**

* Choose where new code is written.
* Decide what level of detail is appropriate.

### Patterns of stratified design

* Pattern 1: Straightforward (простая/несложная) implementation

  The layer structure of stratified design should help us build straightforward implementations.
  When we read a function with a straightforward implementation, the problem the function
  signature presents should be solved at the right level of detail in the body. Too much detail
  is a code smell.

* Pattern 2: Abstraction barrier

  Some layers in the graph provide an interface that lets us hide an important implementation
  detail. These layers help us write code at a higher level and free our limited mental capacity
  to think at a higher level.

* Pattern 3: Minimal interface

  As our system evolves, we want the interfaces to important business concepts to converge to
  a small, powerful set of operations. Every other operation should be defined in terms of those,
  either directly or indirectly.

* Pattern 4: Comfortable layers

  The patterns and practices of stratified design should serve our needs as programmers, who are
  in turn serving the business. We should invest time in the layers that will help us deliver
  software faster and with higher quality. We don't want to add layers for sport. The code and
  its layers of abstraction should feel comfortable to work in.

### Pattern 1: Straightforward implementations

1. Выделить desired (нужные) operations.

2. Visualizing our function calls with a call graph.

Пример такой диаграммы:

```text
    ------------- freeTieClip() --------------
    |             |           |              |
    |             |           v              v
    |             |      make_item()    add_item()
    v             v
array_index    for loop
```

* Arrows represent function calls.
* `array index`, `for loop` - language features.
* `make_item()`, `add_item()` - function calls.
* Functions and built-in language features могут быть расположены на разных уровнях

Замечания по приведенной диаграмме:

* `freeTieClip()` обращается к разным уровням (стрелки указывают на разные уровни)
* The difference in layers makes the implementation less obvious and hard to read.

3.Straightforward (простые/несложные) implementations call functions from similar layers of
abstraction.

In a straightforward implementation, all arrows would be about the same length.

Если из одного слоя идет обращение к разным слоям, то можно ввести промежуточные функции
для "выравнивания" (чтобы обращения были только к функциям одного нижележащего слоя).

<img src="images/ch08_compare_arrows_before.jpg" alt="Compare arrows before"/>

<img src="images/ch08_compare_arrows_after.jpg" alt="Compare arrows after"/>

4. Стрелки между функциями из разных слоев должны быть как можно короче.

5. All functions in a layer should serve (выполнять) the same pupose (функцию/работу).

### Three different zoom levels

1. Global zoom level

  At the global zoom level, we see the entire call graph.

<img src="images/ch08_global_zoom_level.jpg" alt="Global zoom level"/>

2. Layer zoom level

  At the layer zoom level, we start with the level of interest and draw everything it
  points to below it.

<img src="images/ch08_layer_zoom_level.jpg" alt="Layer zoom level"/>

3. Function zoom level

  At the function zoom level, we start with one function of interest and draw everything it
  points to below it.

## Chapter 9. Stratified design: Part 2

An *abstraction barrier* is a layer of functions that hide the implementation.

**Важно**: на диаграмме нет стрелок, которые пересекают abstraction barrier.

### Abstraction barrier. Abstraction barriers hide implementations

Functional programmers strategically employ (используют) abstraction barriers because they
let them think about a problem at a higher level.

The abstraction barrier in this case means the functions above that layer don't need to
know what the data structure is.

Также правильный выбор abstraction barrier позволяет нижним слоям успешно игнорировать
функции из более верхних слоев.

<img src="images/ch09_abstraction_barrier.jpg" alt="Abstraction barriers hide implementations"/>

### When to use abstraction barriers

* To facilitate (для облегчения) changes of implementation.

  * Abstraction barrier позволяет потом изменять нижележащие слои.

  * This property might be useful if you are prototyping something and you still don't
  know how best to implement it.

  * You know something will change; you're just not ready to do it yet.

* To make code easier to write and read.

  * An abstraction barrier that lets you ignore lower code details will make your code
  easier to write.

* To reduce coordination between teams.

  * The abstraction barrier allows teams on either (обеих) side to ignore the details the
  other team handles.

* To mentally focus on the problem at hand.

### When not to use abstraction barriers

* Code in the barrier is lower level, so it's more likely to contain bugs.

* Low-level code is harder to understand.

### Pattern 3: Minimal interface

1. If we add more code to the barrier, we have more to change when we change the
implementation.

2. More functions in an abstraction barrier mean more coordination between teams.

3. A larger interface to our abstraction barrier is harder to keep in your head.

### Идеальный layer, к которому следует стремиться

1. Layer should have as many functions as necessary, but no more.

2. The functions should not have to change, nor should you need to add functions later.

3. The set should be complete, minimal, and timeless.

### Pattern 4: Comfortable layers

Не надо делать слишком "высокие" башни абстракций.

Не надо добавлять слои только ради спорта.

We should invest time in the layers that will help us deliver software faster and with higher
quality.

Если нам комфортно работать с текущими уровнями, значит, скорее всего, они не нуждаются в
дополнительных улучшениях.

### Spreading rule (правило распространения)

Любая функция, которая вызывает action, сама становится action.

<img src="images/ch08_spreading_rule.jpg" alt="Spreading rule"/>

### What does the graph show us about our code?

*Nonfunctional requirements* (**NFR**s) are things like how testable, maintainable, or
reusable the code is.

Рассматриваются три NFRs:

1. *Maintainability* - What code is easiest to change when requirements change?
2. *Testability* - What is most important to test?
3. *Reusability* - What functions are easier to reuse?

### Code at the top of the graph is easier to change

The longer the path from the top to a function, the more expensive that function will be to
change.

If we put code that changes frequently near or at the top, our jobs will be easier. Build
less on top of things that change.

<img src="images/ch09_maintainability.jpg" alt="Maintainability"/>

### Testing code at the bottom is more important

Тесты функций на верхних слоях иерархии имеют меньший вес/значение, т.к. данный функционал
может меняться очень часто.

Наоборот, тесты функций на нижних слоях иерархии имеют больший вес, т.к. здесь изменения
происходят гораздо реже.

<img src="images/ch09_testability.jpg" alt="Testability"/>

### Code at the bottom is more reusable (более многократно используется)

Чем выше функция в иерархии, тем меньше она пригодна для повторного использования.

<img src="images/ch09_reusability.jpg" alt="Reusability"/>

### Summary: What the graph shows us about our code

<img src="images/ch09_arranging_functions.jpg" alt="Arranging functions"/>

## Chapter 10. First-class functions: Part 1

Code smell: Implicit argument in function name.

There are two characteristics to the implicit argument in function name smell:

1. Very similar function implementations
2. Name of function indicates the difference in implementation

### Refactoring #1: Express implicit argument

Steps:

1. Identify the implicit argument in the name of the function.
2. Add explicit argument.
3. Use new argument in body in place of hard-coded value.
4. Update the calling code.

#### Example

Before:

```js
// Пример одной из функций, к которой применяется рефакторинг.
function setPriceByName(cart, name, price) {
    var item = cart[name];
    var newItem = objectSet(item, 'price', price);
    var newCart = objectSet(cart, name, newItem);
    return newCart;
}

// Использование
cart = setPriceByName(cart, "shoe", 13);
cart = setQuantityByName(cart, "shoe", 3);
cart = setShippingByName(cart, "shoe", 0);
cart = setTaxByName(cart, "shoe", 2.34);
```

```js
// Функция из главы 6.
function objectSet(object, key, value) {
    var copy = Object.assign({}, object);
    copy[key] = value;
    return copy;
}
```

After:

```js
function setFieldByName(cart, name, field, value) {
    var item = cart[name];
    var newItem = objectSet(item, field, value);
    var newCart = objectSet(cart, name, newItem);
    return newCart;
}

// Использование
cart = setFieldByName(cart, "shoe", 'price', 13);
cart = setFieldByName(cart, "shoe", 'quantity', 3);
cart = setFieldByName(cart, "shoe", 'shipping', 0);
cart = setFieldByName(cart, "shoe", 'tax', 2.34);
```

### Refactoring #2: Replace body with callback

Steps:

1. Identify the before, body, and after sections.
2. Extract the whole thing into a function.
3. Extract the body section into a function passed as an argument to that function.

#### Example

Примеры обобщенных функций, в которые передается функция (callback) в виде параметра.

```js
function forEach(array, f) {
    for(var i = 0; i < array.length; i++) {
        var item = array[i];
        f(item);
    }
}
```

```js
function withLogging(f) {
    try {
        f();
    } catch (error) {
        logToSnapErrors(error);
    }
}
```

Использование:

```js
function cookAndEat(food) {
    cook(food);
    eat(food);
}

forEach(foods, cookAndEat);

withLogging(function() { saveUserData(user); });
```

## Chapter 11. First-class functions: Part 2

Продолжение предыдущей главы. Применение refactoring: Replace body with callback.

Примеры функций, которые на вход принимают другие функции.

### Copy-on-write for arrays

```js
function withArrayCopy(array, modify) {
    var copy = array.slice();
    modify(copy);
    return copy;
}
```

Использование. (Данные функции являются pure):

```js
// Usage 1. Устанавливает значение в массиве по определенному индексу.
function arraySet(array, idx, value) {
    return withArrayCopy(array, function(copy) {
        copy[idx] = value;
    });
}

// Usage 2. Добавляет элемент в конец массива.
function push(array, elem) {
    return withArrayCopy(array, function(copy) {
        copy.push(elem);
    });
}

// Usage 3. Получает элемент из конца массива вместе с его удалением.
function drop_last(array) {
    return withArrayCopy(array, function(copy) {
        copy.pop();
    });
}

// Usage 4. Удаляет элемент из начала массива (без его получения).
function drop_first(array) {
    return withArrayCopy(array, function(copy) {
        copy.shift();
    });
}
```

### Copy-on-write for objects

```js
function withObjectCopy(object, modify) {
    var copy = Object.assign({}, object);
    modify(copy);
    return copy;
}
```

Использование. (Данные функции являются pure):

```js
// Usage 1.
function objectSet(object, key, value) {
    return withObjectCopy(object, function(copy) {
        copy[key] = value;
    });
}

// Usage 2.
function objectDelete(object, key) {
    return withObjectCopy(object, function(copy) {
        delete copy[key];
    });
}
```

### Try/catch

```js
function tryCatch(f, errorHandler) {
    try {
        return f();
    } catch(error) {
        return errorHandler(error);
    }
}
```

Использование:

```js
tryCatch(sendEmail, logToSnapErrors)

// Вместо
try {
    sendEmail();
} catch(error) {
    logToSnapErrors(error);
}
```

### When

*Мое примечание: применение сомнительно, только как демонстрация.*

```js
function when(test, then) {
    if(test)
        return then();
}
```

Использование:

```js
when(array.length === 0, function() {
    console.log("Array is empty");
});

// Вместо
if(array.length === 0) {
    console.log("Array is empty");
}
```

### If

*Мое примечание: применение сомнительно, только как демонстрация.*

```js
function IF(test, then, ELSE) {
    if(test)
        return then();
    else
        return ELSE();
}
```

Использование:

```js
IF(array.length === 0, function() {
    console.log("Array is empty");
}, function() {
    console.log("Array has something in it.");
});

// Вместо
if (array.length === 0)
    console.log("Array is empty");
else
    console.log("Array has something in it.");
```

### Returning functions from functions

Для создания подобных функций опять применяется refactoring:
Replace body with callback.

Примеры:

```js
// Обобощенная функция. Возвращает функцию, которая вызывает переданную
// функцию и логирует ее ошибки.
function wrapLogging(f) {
    return function(arg) {
        try {
            f(arg);
        } catch (error) {
            logToSnapErrors(error);
        }
    }
}

// Создание функции, которая наделяет переданную функцию новым поведением.
var saveUserDataWithLogging = wrapLogging(saveUserData);
```

Использование:

```js
// "Обычный" режим вызова функции.
try {
    saveUserData(user);
} catch (error) {
    logToSnapErrors(error);
}

// Использование созданной функции.
saveUserDataWithLogging(user);
```

#### Еще примеры функций, которые возвращают функции

1. Функция, которая создает функцию с игнорированием ошибок:

```js
function wrapIgnoreErrors(f) {
    return function(a1, a2, a3) {
        try {
            return f(a1, a2, a3);
        } catch(error) { // error is ignored
            return null;
        }
    };
}
```

2. Функция, которая создает функцию, которая прибавляет число к другому числу:

```js
function makeAdder(n) {
    return function(x) {
        return n + x;
    };
}
```

Использование:

```js
var increment = makeAdder(1);
var plus10 = makeAdder(10);

increment(10)       // 11
plus10(12)          // 22
```

## Chapter 12. Functional iteration

Три главных функциональных инструмента.

### `map()`

```js
// (1) - takes array and function
// (2) - creates a new empty array
// (3) - calls f() to create a new element based on the element from original array
// (4) - push. Adds the new element for each element in the original array
// (5)
function map(array, f) {                    // (1)
    var newArray = [];                      // (2)
    forEach(array, function(element) {
        newArray.push(f(element));          // (3) and (4)
    });
    return newArray;                        // (5)
}
```

<img src="images/ch12_map.jpg" alt="map()"/>

### `filter()`

```js
// (1) - takes array and function
// (2) - creates a new empty array
// (3) - calls f() to check if the element should go in the new array
// (4) - push. Adds the original element if it passes the check
// (5) - returns the new array
function filter(array, f) {                 // (1)
    var newArray = [];                      // (2)
    forEach(array, function(element) {
        if(f(element))                      // (3) and (4)
            newArray.push(element);
    });
    return newArray;                        // (5)
}
```

<img src="images/ch12_filter.jpg" alt="filter()"/>

### `reduce()`

```js
// (1) - takes array, an initial accumulator value, and function
// (2) - initializes the accumulator
// (3) - calls f( ) to calculate the next value of the accumulator,
//       based on current value and current element
// (4) - returns the accumulated value
function reduce(array, init, f) {           // (1)
    var accum = init;                       // (2)
    forEach(array, function(element) {
        accum = f(accum, element);          // (3)
    });
    return accum;                           // (4)
}
```

<img src="images/ch12_reduce.jpg" alt="reduce()"/>

### Реализация `map()` и `filter()` через `reduce()`

Реализации `map()`:

```js
// using only non-mutating operations (inefficient)
function map(array, f) {
    return reduce(array, [], function(ret, item) {
        return ret.concat(f([item]));
    });
}

// using mutating operations (more efficient)
function map(array, f) {
    return reduce(array, [], function(ret, item) {
        ret.push(f(item));
        return ret;
    });
}
```

Реализации `filter()`:

```js
// using only non-mutating operations (inefficient)
function filter(array, f) {
    return reduce(array, [], function(ret, item) {
        if (f(item)) return ret.concat([item]);
        else return ret;
    });
}

// using mutating operations (more efficient)
function filter(array, f) {
    return reduce(array, [], function(ret, item) {
        if(f(item))
            ret.push(item);
        return ret;
    });
}
```

## Chapter 13. Chaining functional tools

### Функция `maxKey()`

Finds the largest value from an array. It uses a function to determine what part of the
value you should compare.

```js
// (1) - function. Pass in callback sayng how to compare values.
function maxKey(array, init, f) {
    return reduce(array, init, function(biggestSoFar, element) {    // (1)
        if(f(biggestSoFar) > f(element)) {
            return biggestSoFar;
        else
            return element;
    });
}
```

Использование:

```js
maxKey(customer.purchases, {total: 0},
    function(purchase) { return purchase.total; }
);
```

Можно выразить `max()` через `maxKey()`:

```js
// (1) - function. Tell maxKey() to compare the whole value unchanged
// (2) - a function that returns uts argument unchanged is called "identity"
function max(array, init) {
    return maxKey(array, init, function(x) {    // (1)
        return x;                               // (2)
    });
}
```

### Clarifying chains, method 1: Name the steps

Пример функции, которая сложна для понимания. Original:

```js
function biggestPurchasesBestCustomers(customers) {
    var bestCustomers = filter(customers, function(customer) {          // step 1
        return customer.purchases.length >= 3;
    });

    var biggestPurchases = map(bestCustomers, function(customer) {      // step 2
        return maxKey(customer.purchases, {total: 0}, function(purchase) {
            return purchase.total;
        });
    });

    return biggestPurchases;
}
```

Refactoring. Extracted a function for each higher-order function and named it:

```js
// steps are shorter and dense with meaning (короткие и ясночитаемые).
function biggestPurchasesBestCustomers(customers) {
    var bestCustomers = selectBestCustomers(customers);         // step 1
    var biggestPurchases = getBiggestPurchases(bestCustomers);  // step 2
    return biggestPurchases;
}

function selectBestCustomers(customers) {
    return filter(...);
}

function getBiggestPurchases(customers) {
    return map(...);
}
```

### Clarifying chains, method 2: Naming the callbacks

Пример функции, которая сложна для понимания. Original:

```js
function biggestPurchasesBestCustomers(customers) {
    var bestCustomers = filter(customers, function(customer) {                  // step 1
        return customer.purchases.length >= 3;
    });

    var biggestPurchases = map(bestCustomers, function(customer) {              // step 2
        return maxKey(customer.purchases, {total: 0}, function(purchase) {
            return purchase.total;
        });
    });

    return biggestPurchases;
}
```

Refactoring. Extract and name the callbacks:

```js
// steps are still short and meaningful
// (1) - callbacks are named
function biggestPurchasesBestCustomers(customers) {
    var bestCustomers = filter(customers, isGoodCustomer);              // step 1, (1)
    var biggestPurchases = map(bestCustomers, getBiggestPurchase);      // step 2, (1)
    return biggestPurchases;
}

function isGoodCustomer(customer) {             // (1)
    return customer.purchases.length >= 3;
}

function getBiggestPurchase(customer) {         // (1)
    return maxKey(customer.purchases, {total: 0}, getPurchaseTotal);
}

function getPurchaseTotal(purchase) {
    return purchase.total;
}
```

### Оптимизация вызовов chains (stream fusion)

#### Two `map()` steps in a row

Original:

```js
var names = map(customers, getFullName);
var nameLengths = map(names, stringLength);
```

Optimized:

```js
var nameLengths = map(customers, function(customer) {
    return stringLength(getFullName(customer));
});
```

#### Two `filter()` steps in a row

Original:

```js
var goodCustomers = filter(customers, isGoodCustomer);
var withAddresses = filter(goodCustomers, hasAddress);
```

Optimized:

```js
var withAddresses = filter(customers, function(customer) {
    return isGoodCustomer(customer) && hasAddress(customer);
});
```

#### `map()` step followed by `reduce()` step

Original:

```js
var purchaseTotals = map(purchases, getPurchaseTotal);
var purchaseSum = reduce(purchaseTotals, 0, plus);
```

Optimized:

```js
var purchaseSum = reduce(purchases, 0, function(total, purchase) {
    return total + getPurchaseTotal(purchase);
});
```

### Chaining tips

#### Make data

The functional tools work best when they work over an entire array of data.
If you find the for loop is working over a subset of the data, try to break that data out
into its own array. Then `map()`, `filter()`, and `reduce()` can make short work of it
(помогут быстро обработать эти отдельные массивы).

#### Operate on the whole array

Стараться обрабатывать массивы в одно действие.
`map()` transforms every element.
`filter()` keeps or removes every element.
`reduce()` combines every element. Make a bold move and process the whole thing.

#### Many small steps

Для сложных алгоритмов их разбиение на более мелькие шаги часто помогает облегчить
понимание их работы и читаемость.

#### Ещё tips

* Replace conditionals with `filter()`

* Извлекайте множество вспомогательных функций таких как `map()`, `filter()` и `reduce()`.

* Experiment to improve

### Debugging tips for chaining

* Стараться использовать понятные имена в функциях, параметрах, переменных.
* Выводить (на экран, консоль, логи, ...) промежуточные результаты.
* Отслеживать используемые типы данных
  * `map()` возвращает массив с элементами того же (редко) или нового типов.
  * `filter()` возвращает массив с элементами того же типа.
  * `reduce()` возвращает значение того же типа, что и initial.

### Some other functional tools

#### `pluck()`

Удобная функция-замена `map()` для более удобного получения поля объекта.

```js
function pluck(array, field) {
    return map(array, function(object) {
        return object[field];
    });
}
```

Usage and variation:

```js
// Usage
var prices = pluck(products, 'price');

// Variation
function invokeMap(array, method) {
    return map(array, function(object) {
        return object[method]();
    });
}
```

#### `concat()`

Удаляет один уровень вложенности у массива:

```js
function concat(arrays) {
    var ret = [];
    forEach(arrays, function(array) {
        forEach(array, function(element) {
            ret.push(element);
        });
    });
    return ret;
}
```

Usage and variation:

```js
// Usage
var purchaseArrays = pluck(customers, "purchases");
var allPurchases = concat(purchaseArrays);

// Variation. Also called mapcat() or flatMap() in some languages
function concatMap(array, f) {
    return concat(map(array, f));
}
```

#### `frequenciesBy()` and `groupBy()`

Подсчет количества и группировка. Данные функции возвращают объекты (хэш-карты):

```js
function frequenciesBy(array, f) {
    var ret = {};
    forEach(array, function(element) {
        var key = f(element);
        if(ret[key]) ret[key] += 1;
        else ret[key] = 1;
    });
    return ret;
}

function groupBy(array, f) {
    var ret = {};
    forEach(array, function(element) {
        var key = f(element);
        if(ret[key]) ret[key].push(element);
        else ret[key] = [element];
    });
    return ret;
}
```

Usage:

```js
var howMany = frequenciesBy(products, function(p) {
    return p.type;
});
```

```text
> console.log(howMany['ties'])      // 4
```

```js
var groups = groupBy(range(0, 10), isEven);
```

```text
> console.log(groups)
{
    true: [0, 2, 4, 6, 8],
    false: [1, 3, 5, 7, 9]
}
```

## Chapter 14. Functional tools for nested data

### Functional tool: `update()`

`update()` let's us take a function that operates on a single value
and apply it in place inside of an object (treated as hash maps):

```js
// (1) - takes the object to modify
//       the location of the value (key)
//       the modify operation - function to call
// (2) - returns the modified object (copy-on-write)
function update(item, field, modify) {                  // (1)
    var value = item[field];                            // get
    var newValue = modify(value);                       // modify
    var newItem = objectSet(item, field, newValue);     // set
    return newItem;                                     // (2)
}
```

### Steps for replace get, modify, set with `update()`

1. Identify get, modify, and set.

2. Replace with `update()`, passing modify as callback.

### Functional tool: `update2()`, `update3()`, etc

`update2` - the 2 means nested twice. Modify a value nested twice within objects.

```js
function update2(object, key1, key2, modify) {
    return update(object, key1, function(value1) {
        return update(value1, key2, modify);
    });
}
```

Похожим образом получаются остальные функции `update3()`, `update4()`, ...
`update3()`:

```js
function update3(object, key1, key2, key3, modify) {
    return update(object, key1, function(object2) {
        return update2(object2, key2, key3, modify);
    });
}
```

### Functional tool: `nestedUpdate()`

It takes an object, a path of keys to follow into the nesting of the objects, and a
function to call on the value once it is found.
`nestedUpdate()` works on paths of any length, including zero. It is *recursive*.

```js
// (1) - base case (path of zero length)
// (2) - make progress toward (по направлению к) base case (by dropping one path element)
// (3) - recursive case
function nestedUpdate(object, keys, modify) {
    if(keys.length === 0)
        return modify(object);                              // (2)
    var key1 = keys[0];
    var restOfKeys = drop_first(keys);                      // (2)
    return update(object, key1, function(value1) {
        return nestedUpdate(value1, restOfKeys, modify);    // (3)
    });
}
```

### The anatomy of safe recursion

1. Base case
2. Recursive case
3. Progress toward (по направлению к) the base case

## Chapter 15. Isolating timelines

### The two fundamentals of timeline diagrams

0. **Only actions** need to be in timelines. **Calculations can be left out**
because they don't depend on when they are run.

1. If two actions occur in order, put them in the same timeline.

2. If two actions can happen at the same time or out of order (не по порядку),
they belong (находятся) in separate timelines.

### Two tricky details about the order of actions

1. `++` and `+=` are really three steps

```js
total++;
```

This single operator does three steps:

```js
var temp = total;       // read (action)
temp = temp + 1;        // addition (calculation)
total = temp;           // write (action)
```

Timeline:

```text
Read total
    |
Write total
```

2. Arguments are executed before the function is called

```js
console.log(total);
```

Equivalent code:

```js
var temp = total;
console.log(temp);
```

Timeline:

```text
Read total
    |
console.log()
```

### Step 1. Drawing timeline

1. Identify the actions. Ignore calculations.
2. Draw each action, whether sequential or parallel.
3. Simplify using platform-specific knowledge.

### Asynchronous calls require new timelines

### Different languages, different threading models

* Single-threaded, synchronous. (example: PHP)

* Single-threaded, asynchronous. (example: JavaScript)

* Multi-threaded. (example: Java, Python, Ruby, C, C#)

* Message-passing processes. (example: Erlang, Elixir)

### Timeline diagrams capture the two kinds of sequential code

1. Code that can be interleaved (чередоваться/меняться местами).

Any amount of time can pass between two actions.

```text
action 1
   |
action 2
```

2. Code that cannot be interleaved

Two actions run one after the other. Порядок их выполнения не может быть изменен.

```text
action 1
action 2
```

### Principles of working with timelines

1. Fewer (меньшее количество) timelines are easier.

2. Shorter timelines are easier.

3. Sharing fewer (меньшее число) resources is easier.

4. Coordinate when resources are shared. (Необходима координация использования общих
ресурсов между различными timeline'ами.)

5. Manipulate time as a first-class concept. (создание повторно используемых объектов
для манипуляций с timeline).

### Simplifying the timeline

1. Consolidate all actions on a single timeline.

2. Consolidate timelines that end by creating one new timeline.

### Timelines that share resources can cause problems

We can remove problems by not sharing resources

#### Converting a global variable to a local one

1. Identify the global variable we would like to make local.

2. Replace the global variable with a local variable.

#### Converting a global variable to an argument

1. Identify the implicit (неявный) input.

2. Replace the implicit (неявный) input with an argument.

## Chapter 16. Sharing resources between timelines

*Мое примечание: аццкий ад с этими callback.*

A *concurrency primitive* is a piece of reusable functionality that helps share
resources across timelines.

### Using `Queue` as concurrency primitive

```js
// (1) - Queue() is very generic, so the variable names are generic as well
// (2) - we allow done() to accept an argument.
// (3) - set up asynchronous call to item.callback.
function Queue(worker) {                // (1)
    var queue_items = [];               // (1)
    var working = false;

    function runNext() {
        if(working)
            return;
        if(queue_items.length === 0)
            return;
        working = true;
        var item = queue_items.shift();
        worker(item.data, function(val) {       // (2)
            working = false;
            setTimeout(item.callback, 0, val);  // (3)
            runNext();
        });
    }

    return function(data, callback) {
        queue_items.push({
            data: data,
            callback: callback || function(){}
        });
        setTimeout(runNext, 0);
    };
}
```

Usage:

```js
// (1) - cart will get the item data; we call done() when we're done.
// (2) - here we know the specifics of what we're doing, so we use specific variable names.
function calc_cart_worker(cart, done) {         // (1)
    calc_cart_total(cart, function(total) {     // (2)
        update_total_dom(total);
        done(total);
    });
}

var update_total_queue = Queue(calc_cart_worker);
```

### Using `DroppingQueue` as concurrency primitive

Выполняет только ограниченное число/(последний) action из всех action в очереди.

По сути, в `DroppingQueue` небольшое улучшение по сравнению с предыдущим `Queue`.

*Мое примечание: DroppingQueue - программный аналог бездребезговой кнопки.*

```js
// max - pass the max of tasks to keep. 1 - будет выполнен только 1 task.
// (1) - keep dropping items from the front until we are under or at max.
function DroppingQueue(max, worker) {
    var queue_items = [];
    var working = false;

    function runNext() {
        if(working)
            return;
        if(queue_items.length === 0)
            return;
        working = true;
        var item = queue_items.shift();
        worker(item.data, function(val) {
            working = false;
            setTimeout(item.callback, 0, val);
            runNext();
        });
    }

    return function(data, callback) {
        queue_items.push({
            data: data,
            callback: callback || function(){}
        });
        while(queue_items.length > max)      // (1)
            queue_items.shift();             // (1)
        setTimeout(runNext, 0);
    };
}
```

Usage:

```js
function calc_cart_worker(cart, done) {
    calc_cart_total(cart, function(total) {
        update_total_dom(total);
        done(total);
    });
}

// 1 - drop all but one (будет вызван только послений worker)
var update_total_queue = DroppingQueue(1, calc_cart_worker);
```

### Principle: Use real-world sharing as inspiration (источники для вдохновения)

Examples:

* *Locks on bathroom* doors enable a one-person-at-a-time discipline.
* *Public libraries* (book pools) allow a community to share many books.
* *Blackboards* allow one teacher (one writer) to share information with an entire class
(many readers).

## Chapter 17. Coordinating timelines

### Recap. Implicit versus explicit model of time

1. Sequential statements execute in sequential order.

<img src="images/ch17_seq_statements.jpg" alt="Sequential statements"/>

2. Steps in two different timelines can occur in left-first or right-first order.

<img src="images/ch17_steps_timelines.jpg" alt="Steps in two different timelines"/>

3. Asynchronous events are called in new timelines.

<img src="images/ch17_async_events.jpg" alt="Asynchronous events"/>

4. An action is executed as many times as you call it.

<img src="images/ch17_action_exec.jpg" alt="An action is executed as many times as you call it"/>

### A concurrency primitive for cutting timelines. `Cut()`

In JavaScript (as single thread): Every timeline will call that function when it's done.
Every time the function is called, we increment the number of times it has been called.
Then, when the last function calls it, it will call a callback:

```js
// num - number of timelines to wait for
// callback - the callback to execute when they are all done
function Cut(num, callback) {
    var num_finished = 0;            // initialize the count to zero
    return function() {          // the returned function is called at the end of each timeline
        num_finished += 1;       // each time function is called, we increment the count
        if(num_finished === num)
            callback();          // when the last timeline finishes, we call the callback
    };
}
```

### A primitive to call something just once. `JustOnce()`

Functionality thats perform an action just once, no matter how many times the code may call
that action.

```js
function JustOnce(action) {         // pass in an action
    var alreadyCalled = false;      // remember if we've called it already
    return function(a, b, c) {
        if(alreadyCalled) return;   // exit early if we've called it before
        alreadyCalled = true;       // we're about to call it, so remember
        return action(a, b, c);     // call the action, passing through the arguments
    };
}
```

### Recap. Concurrency primitives

#### `Queue()`

Items added to the queue are processed in a separate, single timeline. Each item is
handled in order to completion before the next is started:

<img src="images/ch17_queue.jpg" alt="Queue()"/>

#### `Cut()`

Call a callback in a new timeline only after all timelines have completed:

<img src="images/ch17_cut.jpg" alt="Cut()"/>

#### `JustOnce()`

An action wrapped in `JustOnce()` will only be executed once, even if the wrapped
function is called multiple times:

<img src="images/ch17_just_once.jpg" alt="JustOnce()"/>

#### `DroppingQueue()`

This is like a Queue(), but will skip tasks if they build up quickly:

<img src="images/ch17_dropping_queue.jpg" alt="DroppingQueue()"/>

## Chapter 18. Reactive and onion architectures

* *Reactive architecture* is used at the level of individual sequences of actions.

It helps decouple cause from effect, which can untangle (распутать) some confusing
parts of our code.

* *Onion architecture* operates at the level of an entire service.

<img src="images/ch18_two_architectures.jpg" alt="Reactive and Onion architectural patterns"/>

### What is reactive architecture?

Reactive architecture specify what happens in response to events. It is very useful
in web services and UIs.

<img src="images/ch18_event_handlers.jpg" alt="Examples of event handlers"/>

Event handlers let you say, "When X happens, do Y, Z, A, B, and C." In the reactive
architecture, we break up the typical step-by-step handler function into a series of
handlers that respond (откликаются/срабатывают) to the previous one.

<img src="images/ch18_reactive_handler.jpg" alt="Reactive architecture"/>

### `ValueCell`. Reactive version

`ValueCell` simply wrap a variable with two simple operations.
One reads the current value `(val())`. The other updates the current value `(update())`.

*Watchers* are handler functions that get called every time the state changes.

```js
// (1) - hold one immutable value (can be a collection)
// (2) - keep a list of watchers
// (3) - get current value
// (4) - modify value by applying a function to current value (swapping pattern)
// (5) - call watchers when value changes
// (6) - add a new watcher
function ValueCell(initialValue) {
    var currentValue = initialValue;                    // (1)
    var watchers = [];                                  // (2)
    return {
        val: function() {                               // (3)
            return currentValue;
        },
        update: function(f) {                           // (4)
            var oldValue = currentValue;
            var newValue = f(oldValue);
            if(oldValue !== newValue) {
                currentValue = newValue;
                forEach(watchers, function(watcher) {   // (5)
                    watcher(newValue);
                });
            }
        },
        addWatcher: function(f) {                       // (6)
            watchers.push(f);
        }
    };
}
```

### `FormulaCell` calculate derived values

`FormulaCell` watch another cell and recalculate it's value when the upstream cell changes.

```js
// (1) - reuse the machinery of ValueCell
// (2) - add a watcher to recompute the current value of this cell
// (3) - val() and addWatcher() delegate to myCell
// (4) - FormulaCell has no way to change value directly
function FormulaCell(upstreamCell, f) {
    var myCell = ValueCell(f(upstreamCell.val()));          // (1)
    upstreamCell.addWatcher(function(newUpstreamValue) {    // (2)
        myCell.update(function(currentValue) {
            return f(newUpstreamValue);
        });
    });
    return {
        val: myCell.val,                    // (3)
        addWatcher: myCell.addWatcher       // (3), (4)
    };
}
```

### `ValueCell` consistency guidelines (рекомендации для согласованности)

* Initialize with a valid value.
* Pass a calculation to `update()` (never an action (никогда не использовать action)).
* That calculation should return a valid value if passed a valid value.

### Mutable state in functional programming

<img src="images/ch18_mutable_state.jpg" alt="Mutable state in functional programming"/>

### How reactive architecture reconfigures systems

<img src="images/ch18_typical_vs_reactive.jpg" alt="Typical vs Reactive architecture"/>

Reactive architecture has three major effects on our code:

1. Decouples effects from their causes (причины).
2. Treats series of steps as pipelines.
3. Creates flexibility in your timeline.

### What is the onion architecture?

<img src="images/ch18_onion_architecture.jpg" alt="Onion architecture"/>

Rules of onion architecture:

1. Interaction with the world is done exclusively in the interaction layer.
2. Layers call in toward (в направлении) the center.
3. Layers don't know about layers outside of themselves.

### Traditional layered architecture vs functional architecture

<img src="images/ch18_traditional_vs_functional.jpg" alt="Traditional and functional architecture"/>

## Links

* [Erlang](https://www.erlang.org)

* [Elixir](https://elixir-lang.org)

* [Lodash: Functional tools for JavaScript](https://lodash.com/docs)

* [Laravel Collections. Functional tools for PHP](https://laravel.com/docs/collections#available-methods)

* Clojure standard library

  * [ClojureDocs quick reference](https://clojuredocs.org/quickref#sequences)

  * [Official docs](https://clojure.github.io/clojure/clojure.core-api.html)

* [Haskell Prelude](http://www.cse.chalmers.se/edu/course/TDA555/tourofprelude.html)

* [The Reactive Manifesto](https://www.reactivemanifesto.org)

  Information about the reactive architecture.

* [ReactiveX](https://reactivex.io)

  An API for asynchronous programming with observable streams. (Для различных языков).

* [Kafka](https://kafka.apache.org), [RabbitMQ](https://www.rabbitmq.com)

  Streaming services. Implement a reactive architecture at a larger scale in your system between
  separate services.

* Popular functional languages

  * [Clojure](https://clojure.org)

  Clojure runs on the Java Virtual Machine and JavaScript (in the form of ClojureScript).

  * [Elixir](https://elixir-lang.org)

  Elixir runs on the Erlang Virtual Machine. It uses actors to manage concurrency.

  * [Swift](https://swift.org)

  Swift is Apple's open source, flagship language.

  * [Kotlin](https://kotlinlang.org)

  Kotlin combines object-oriented and functional programming into one JVM language.

  * [Haskell](https://haskell.org)

  Haskell is a statically typed language used in academia, startups, and enterprises alike.

  * [Erlang](https://erlang.org)

  Erlang was built for fault tolerance. It uses actors for concurrency.

  * [Elm](https://elm-lang.org)

  Elm is statically typed and used for frontend web applications that
  compile to JavaScript.

  * [Scala](https://scala-lang.org)

  Scala combines object-oriented and functional programming into one language.
  It runs on the Java Virtual Machine and JavaScript.

  * [F#](https://fsharp.org)

  F# is statically typed and runs on the Microsoft Common Language Runtime.

  * [Rust](https://rust-lang.org)

  Rust is a system language with a powerful type system designed to prevent memory
  leaks and concurrency errors.

  * [PureScript](https://www.purescript.org)

  PureScript is a Haskell-like language that compiles to JavaScript to run in the
  browser.

  * [Racket](https://racket-lang.org)

  Racket has a rich history and a large and vibrant community.

  * [Reason](https://reasonml.github.io)

  Reason is statically typed and compiles to JavaScript and native assembly.
