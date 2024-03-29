# Grokking Simplicity

## Part 2: First-class abstractions

## Chapter 10. First-class functions: Part 1

### Vocab

A *code smell* is a characteristic of a piece of code that might be a symptom of
deeper problems.

A *first-class value* can be used just like all of the other values in your language.

*Data orientation* is a style of programming that uses generic data structures to
represent facts about events and entities.

*Higher-order functions* take other functions as arguments or return functions as
their return values.

*Anonymous functions* are functions without names. They can be written inline - right
where they are used.

In the JavaScript world, functions passed as arguments are often called *callbacks*,
but we also hear the same term outside of the JavaScript community. The function you
are passing the callback to is expected to call the function. Other communities might
call these handler functions. Experienced functional programmers are so used to passing
functions as arguments, they often don't need a special name for them.

An *inline function* is defined where it is used. For example, a function might be
defined in the argument list.

An *anonymous function* is a function without a name. That usually happens when we
define the function inline.

### Code smell: Implicit (неявный) argument in function name

There are two characteristics to the implicit *argument in function name smell*:

1. Very similar function implementations
2. Name of function indicates the difference in implementation

### Refactoring: Express (выразить) implicit argument

1. Identify the implicit argument in the name of the function.
2. Add explicit argument.
3. Use new argument in body in place of hard-coded value.
4. Update the calling code.

Пример. Есть функции:

```js
// Функции
function multiplyByFour(x) {
    return x * 4;
}

function multiplyByPi(x) {
    return x * 3.14159;
}

// Их вызовы
a = multiplyByFour(7);
b = multiplyByPi(2);
```

Шаги:

1. Identify implicit argument:

  multiplyBy**Four**, multiplyBy**Pi**

2. Add explicit argument.

```js
function multiply(x, y) {
    return x * 4;
}

function multiply(x, y) {
    return x * 3.14159;
}
```

3. Use new argument in body.

```js
function multiply(x, y) {
    return x * y;
}
```

4. Update calling code.

```js
a = multiply(7, 4);
b = multiply(2, 3.14159);
```

### First-class functions can replace any syntax

#### Шаги

1. Wrap code in functions.
2. Rename to be more generic.
3. Express implicit argument.
4. Extract function.
5. Express implicit argument.

Пример. Есть похожие куски кода:

```js
// Preparing and eating
for(var i = 0; i < foods.length; i++) {         // Похожие строки
    var food = foods[i];                        // Похожие строки
    cook(food);
    eat(food);
}

// Washing up
for(var i = 0; i < dishes.length; i++) {        // Похожие строки
    var dish = dishes[i];                       // Похожие строки
    wash(dish);
    dry(dish);
    putAway(dish);
}
```

Шаги по рефакторингу:

1. Выделить эти куски кода в функции и дать им descriptive names. И потом их вызвать.

```js
function cookAndEatFoods() {
    for(var i = 0; i < foods.length; i++) {
        var food = foods[i];
        cook(food);
        eat(food);
    }
}

function cleanDishes() {
    for(var i = 0; i < dishes.length; i++) {
        var dish = dishes[i];
        wash(dish);
        dry(dish);
        putAway(dish);
    }
}

cookAndEatFoods();
cleanDishes();
```

2. Переименование похожих переменных.

```js
function cookAndEatFoods() {                    // food -> item
    for(var i = 0; i < foods.length; i++) {
        var item = foods[i];
        cook(item);
        eat(item);
    }
}

function cleanDishes() {                        // dish -> item
    for(var i = 0; i < dishes.length; i++) {
        var item = dishes[i];
        wash(item);
        dry(item);
        putAway(item);
    }
}

cookAndEatFoods();
cleanDishes();
```

3. Implicit argument in function name refactoring.

  * Change name to reflect that it is generic
  * Add explicit array argument
  * Pass the arrays in

```js
function cookAndEatArray(array) {                    // foods -> array
    for(var i = 0; i < array.length; i++) {
        var item = array[i];
        cook(item);
        eat(item);
    }
}

function cleanArray(array) {                        // dishes -> array
    for(var i = 0; i < array.length; i++) {
        var item = array[i];
        wash(item);
        dry(item);
        putAway(item);
    }
}

cookAndEatArray(foods);
cleanArray(dishes);
```

4. Extract body of functions

```js
function cookAndEatArray(array) {
    for(var i = 0; i < array.length; i++) {
        var item = array[i];
        cookAndEat(item);       // Call extracted function
    }
}

function cookAndEat(food) {     // Extracted function
    cook(food);
    eat(food);
}

function cleanArray(array) {
    for(var i = 0; i < array.length; i++) {
        var item = array[i];
        clean(item);            // Call extracted function
    }
}

function clean(dish) {          // Extracted function
    wash(dish);
    dry(dish);
    putAway(dish);
}

cookAndEatArray(foods);
cleanArray(dishes);
```

5. Передача тела функции через аргумент

```js
function operateOnArray(array, f) {               // Rename to something generic
    for(var i = 0; i < array.length; i++) {
        var item = array[i];
        f(item);                                  // Call extracted function
    }
}

function cookAndEat(food) {            // Extracted function
    cook(food);
    eat(food);
}

function operateOnArray(array, f) {               // Rename to something generic
    for(var i = 0; i < array.length; i++) {
        var item = array[i];
        f(item);                                  // Call extracted function
    }
}

function clean(dish) {                 // Extracted function
    wash(dish);
    dry(dish);
    putAway(dish);
}

operateOnArray(foods, cookAndEat);    // Add argument to calling code
operateOnArray(dishes, clean);             // Add argument to calling code
```

6. Удаление лишних функций, переименования. Итоговый результат.

```js
function forEach(array, f) {                    // operateOnArray -> forEach
    for(var i = 0; i < array.length; i++) {
        var item = array[i];
        f(item);
    }
}

function cookAndEat(food) {
    cook(food);
    eat(food);
}

function clean(dish) {
    wash(dish);
    dry(dish);
    putAway(dish);
}

forEach(foods, cookAndEat);    // operateOnArray -> forEach
forEach(dishes, clean);        // operateOnArray -> forEach
```

Функция `forEach()` - это *higher-order function* (см. Vocab).

### Refactoring: Replace body with callback

1. Identify the before, body, and after sections.
2. Extract the whole thing into a function.
3. Extract the body section into a function passed as an argument to that function.

Пример:

Шаг 0. Начальное состояние. Same catch everywhere

```js
try {
    saveUserData(user);
} catch (error) {
    logToSnapErrors(error);
}

try {
    fetchProduct(productId);
} catch (error) {
    logToSnapErrors(error);
}
```

Шаг 1. Identify the before, body, and after sections.

```js
try {                           // before
    saveUserData(user);         // body
} catch (error) {               // after
    logToSnapErrors(error);
}

try {                           // before
    fetchProduct(productId);    // body
} catch (error) {               // after
    logToSnapErrors(error);
}
```

Шаг 2. Extract the whole thing into a function.

```js
try {                           // Original
    saveUserData(user);
} catch (error) {
    logToSnapErrors(error);
}

function withLogging() {        // After function extraction
    try {
        saveUserData(user);
    } catch (error) {
        logToSnapErrors(error);
    }
}

withLogging();      // Call withLogging() after we define it
```

Шаг 3. Extract the body section into a function passed as an argument to that function.

Current:

```js
function withLogging() {
    try {
        saveUserData(user);     // we can pull out this part into a callback
    } catch (error) {
        logToSnapErrors(error);
    }
}

withLogging();
```

After extracting callback:

```js
function withLogging(f) {       // f indicates a function
    try {
        f();                    // we call the function in place of the old body
    } catch (error) {
        logToSnapErrors(error);
    }
}

withLogging(function() {        // we have to pass in the body now
    saveUserData(user);         // one-line anonymous function
});
```

### What is this syntax?

#### 1. Globally defined

```js
function saveCurrentUserData() {        // define the function globally
    saveUserData(user);
}

withLogging(saveCurrentUserData);       // pass the function by name
```

#### 2. Locally defined

Функция видна и доступна только внутри scope, где она определена.

```js
function someFunction() {
    var saveCurrentUserData = function() {      // local function
        saveUserData(user);
    };
    withLogging(saveCurrentUserData);           // pass the function by name
}
```

#### 3. Defined inline

This function has no name (*anonymous function*). *Inline* means we defined it where it was
used.

```js
withLogging(function() { saveUserData(user); });
```

### Examples of non-first-class things in JavaScript

*(Примеры сущностей/конструкций, которые нельзя использовать для передачи в качестве входных и выходных параметров в функциях)*

1. Arithmetic operators
2. For loops
3. If statements
4. Try/catch blocks

### Examples of things you can do with a first-class value

1. Assign it to a variable.
2. Pass it as an argument to a function.
3. Return it from a function.
4. Store it in an array or object.

### Summary

* First-class values are anything that can be stored in a variable, passed as an argument,
and returned from functions. A first-class value can be manipulated by code.

* Many parts of a language are not first-class. We can wrap those parts in functions that do
the same thing to make them first-class.

* Some languages have first-class functions that let you treat functions as first-class values.
First-class functions are necessary for doing this level of functional programming.

* Higher-order functions are functions that take other functions as arguments (or that
return a function). Higher-order functions let us abstract over varying behavior.

* Implicit argument in function name is a code smell where the difference between
functions is named in the function name. We can apply *express implicit argument* to
make the argument first-class instead of an inaccessible part of the function name.

* We can apply the refactoring called replace body with callback to abstract over behavior. It
creates a first-class function argument that represents the behavioral difference between
two functions.

## Chapter 11. First-class functions: Part 2

### One code smell and two refactorings (from previous chapter)

Code smell: *Implicit argument in function name*

1. There are very similar function implementations.
2. The name of the function indicates the difference in implementation.

Refactoring 1: *Express implicit argument*

1. Identify the implicit argument in the name of the function.
2. Add explicit argument.
3. Use new argument in body in place of hard-coded value.
4. Update the calling code.

Refactoring 2: *Replace body with callback*

1. Identify the before, body, and after sections.
2. Extract the whole thing into a function.
3. Extract the body section into a function passed as an argument to that function.

### Refactoring copy-on-write

Steps of copy-on-write:

1. Make a copy.         // before
2. Modify the copy.     // body
3. Return the copy.     // after

#### Example. Refactoring copy-on-write for arrays

Шаг 1. Identify before, body, after.

```js
function arraySet(array, idx, value) {      function arraySet(array, idx, value) {
    var copy = array.slice();                   var copy = array.slice();           // before
    copy[idx] = value;                          copy.push(elem);                    // body
    return copy;                                return copy;                        // after
}                                           }

function drop_last(array) {                 function drop_first(array) {
    var array_copy = array.slice();             var array_copy = array.slice();     // before
    array_copy.pop();                           array_copy.shift();                 // body
    return array_copy;                          return array_copy;                  // after
}                                           }
```

Шаг 2. Extract function.

Original:

```js
function arraySet(array, idx, value) {
    var copy = array.slice();               // extract to a function
    copy[idx] = value;
    return copy;
}
```

After function extraction:

```js
function arraySet(array, idx, value) {
    return withArrayCopy(array);
}

function withArrayCopy(array) {
    var copy = array.slice();
    copy[idx] = value;          // idx - not defined in this scope
    return copy;
}
```

Шаг 3. Extract extract out the body into a callback.

Current:

```js
function arraySet(array, idx, value) {
    return withArrayCopy(array);
}

function withArrayCopy(array) {
    var copy = array.slice();
    copy[idx] = value;          // make the body an argument and pass it in
    return copy;
}
```

After extracting callback:

```js
function arraySet(array, idx, value) {
    return withArrayCopy(array, function(copy) {
        copy[idx] = value;
    });
}

function withArrayCopy(array, modify) {         // modify - callback
    var copy = array.slice();
    modify(copy);
    return copy;
}
```

Done. Comparing:

```js
// Before refactoring                       // After refactoring
function arraySet(array, idx, value) {      function arraySet(array, idx, value) {
    var copy = array.slice();                   return withArrayCopy(array, function(copy) {
    copy[idx] = value;                              copy[idx] = value;
    return copy;                                });
}                                           }

                                            // Reusable function
                                            function withArrayCopy(array, modify) {
                                                var copy = array.slice();
                                                modify(copy);
                                                return copy;
                                            }
```

Example of using our reusable function in sorting:

```js
var sortedArray = withArrayCopy(array, function(copy) {
    SuperSorter.sort(copy);
});
```

### Reusable functions examples

**1.** Copy-on-write for arrays.

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

**2.** Copy-on-write for objects.

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

**3.** Try/catch

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

**4.** When (*мое примечание: применение сомнительно, только как демонстрация*)

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

**5.** If (*мое примечание: применение сомнительно, только как демонстрация*)

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

Есть похожие функции:

```js
function saveUserDataWithLogging(user) {
    try {
        saveUserDataNoLogging(user);
    } catch (error) {
        logToSnapErrors(error);
    }
}

function fetchProductWithLogging(productId) {
    try {
        fetchProductNoLogging(productId);
    } catch (error) {
        logToSnapErrors(error);
    }
}
```

Remove the names and make them anonymous:

```js
function(arg) {                         // before
    try {
        saveUserDataNoLogging(arg);     // body
    } catch (error) {                   // after
        logToSnapErrors(error);
    }
}
```

Refactoring - replace body with callback:

```js
// Было
function(arg) {
    try {
        saveUserDataNoLogging(arg);
    } catch (error) {
        logToSnapErrors(error);
    }
}

// Стало
function wrapLogging(f) {               // takes function as argument
    return function(arg) {              // returns a function
        try {
            f(arg);
        } catch (error) {
            logToSnapErrors(error);
        }
    }
}

// Call wrapLogging() with the function we want to transform
var saveUserDataWithLogging = wrapLogging(saveUserDataNoLogging);
var fetchProductWithLogging = wrapLogging(fetchProductNoLogging);
```

Готово. Сравнение использования:

```js
// Начальный вариант            // Конечный вариант
try {                           saveUserDataWithLogging(user)
    saveUserData(user);
} catch (error) {
    logToSnapErrors(error);
}
```

Для конечного варианта "за кадром" используется:

```js
function wrapLogging(f) {
    return function(arg) {
        try {
            f(arg);
        } catch (error) {
            logToSnapErrors(error);
        }
    }
}

var saveUserDataWithLogging = wrapLogging(saveUserData);
```

**Пример**. Функция, которая создает функцию с игнорированием ошибок:

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

Еще **пример**. Функция, которая создает функцию, которая прибавляет число к другому числу:

```js
function makeAdder(n) {
    return function(x) {
        return n + x;
    };
}
```

Использвание:

```js
var increment = makeAdder(1);
var plus10 = makeAdder(10);

increment(10)    // 11
plus10(12)       // 22
```

### Summary

* Higher-order functions can codify patterns and disciplines that otherwise we would have
to maintain manually. Because they are defined once, we can get them right once and
can use them many times.

* We can make functions by returning them from higher-order functions. The functions
can be used just like normal by assigning them to a variable to give them a name.

* Higher-order functions come with a set of tradeoffs. They can remove a lot of
duplication, but sometimes they cost readability. Learn them well and use them wisely.

## Chapter 12. Functional iteration

### Vocab

An *inline function* is a function that is defined where it is used instead of named and
referred to later.

An *anonymous function* is a function without a name. That usually happens when we define the
function inline.

*Predicates* are functions that return true or false. They are useful for passing to
`filter()` and other higher-order functions.

### Deriving (получение) `map()` from examples

Original:

```js
function emailsForCustomers(customers, goods, bests) {
    var emails = [];
    forEach(customers, function(customer) {      // extract the forEach() into map()
        var email = emailForCustomer(customer, goods, bests);
        emails.push(email);
    });
    return emails;
}
```

Replaced with callback:

```js
function emailsForCustomers(customers, goods, bests) {
    return map(customers, function(customer) {              // body is now passed as callback
        return emailForCustomer(customer, goods, bests);
    });
}

function map(array, f) {                        // callback argument
    var newArray = [];
        forEach(array, function(element) {
            newArray.push(f(element));          // call callback here
    });
    return newArray;
}
```

### Functional tool: `map()`

One of three "functional tools":

```js
// (1) - calls f() to create a new element based on the element from original array
// (2) - push. Adds the new element for each element in the original array
function map(array, f) {                    // takes array and function
    var newArray = [];                      // creates a new empty array
    forEach(array, function(element) {
        newArray.push(f(element));          // (1) and (2)
    });
    return newArray;        // returns the new array
}
```

<img src="images/ch12_map.jpg" alt="map()"/>

#### Examples. Using `map()`

**Example 1**

We have: Array of customers

We want: Array of their email addresses

Function: Takes one customer and returns their email address

```js
map(customers, function(customer) {
    return customer.email;
});
```

**Example 2**

We want: Array of customers: first name, last name, address.

```js
map(customers, function(customer) {
    return {
        firstName : customer.firstName,     // create and return new object
        lastName : customer.lastName,
        address : customer.address
    };
});
```

### Deriving (получение) `filter()` from examples

Original:

```js
function selectBestCustomers(customers) {
    var newArray = [];
    forEach(customers, function(customer) {     // extract the forEach() into filter()
        if(customer.purchases.length >= 3)
            newArray.push(customer);
    });
    return newArray;
}
```

Replaced with callback:

```js
function selectBestCustomers(customers) {
    return filter(customers, function(customer) {
        return customer.purchases.length >= 3;    // wrap the expression in a function
    });                                           // and pass it as argument
}

function filter(array, f) {
    var newArray = [];
    forEach(array, function(element) {
        if(f(element))                      // test expression now contained in callback
            newArray.push(element);
    });
    return newArray;
}
```

### Functional tool: `filter()`

`reduce()` can be also known as: `fold()`, `Aggregate()`, etc.
There are sometimes variations like `foldLeft()` and `foldRight()`, which indicate the
direction in which you process the list.

Second of three "functional tools":

```js
// (1) - calls f() to check if the element should go in the new array
// (2) - push. Adds the original element if it passes the check
function filter(array, f) {                 // takes array and function
    var newArray = [];                      // creates a new empty array
    forEach(array, function(element) {
        if(f(element))                      // (1) and (2)
            newArray.push(element);
    });
    return newArray;        // returns the new array
}
```

<img src="images/ch12_filter.jpg" alt="filter()"/>

#### Examples. Using `filter()`

**Example 1**

We have: Array of customers

We want: Array customers who have zero purchases

Function: Takes one customer and returns true if they have zero purchases

```js
filter(customers, function(customer) {
    return customer.purchases.length === 0;
});
```

**Example 2**

We want: Array of customers with email. Exclude customers without email.

```js
var emailsWithoutNulls = filter(emailsWithNulls, function(email) {
    return email !== null;
});
```

**Example 3**

We want: Array of customers, взять каждого третьего для тестовой группы.

```js
var testGroup = filter(customers, function(customer) {
    return customer.id % 3 === 0;
});
```

### Deriving (получение) `reduce()` from examples

Original:

```js
function countAllPurchases(customers) {
    var total = 0;
    forEach(customers, function(customer) {     // extract the forEach() into reduce()
        total = total + customer.purchases.length;
    });
    return total;
}
```

Replaced with callback:

```js
// (1) - 0. Initial value
// (2) - function. Callback function
function countAllPurchases(customers) {
    return reduce(
        customers, 0, function(total, customer) {       // (1) and (2)
            return total + customer.purchase.length;
        }
    );
}

function reduce(array, init, f) {           // init - initial value
    var accum = init;
    forEach(array, function(element) {
        accum = f(accum, element);          // two arguments to callback
    });
    return accum;
}
```

### Functional tool: `reduce()`

Third of three "functional tools":

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

#### Examples. Using `reduce()`

**Example 1**

We have: Array of strings

We want: One string that is the original strings concatenated

Function: Takes an accumulated string and the current string from the array to concatenate

```js
// strings - the array of strings to reduce
// "" - initial value is empty string
// function - pass in a function that does the concatenation
reduce(strings, "" , function(accum, string) {
    return accum + string;
});
```

**Examples 2, 3, 4**

```js
// add up all numbers in the array
function sum(numbers) {
    return reduce(numbers, 0, function(total, num) {
        return total + num;
    });
}
```

```js
// multiply all numbers in the array
function product(numbers) {
    return reduce(numbers, 1, function(total, num) {
        return total * num;
    });
}
```

Find smallest and largest numbers in an array.

`Number.MIN_VALUE` and `Number.MAX_VALUE` - smallest and largest numbers possible in
JavaScript.

```js
// return the smallest number in the array
// (or Number.MAX_VALUE if the array is empty)
function min(numbers) {
    return reduce(numbers, Number.MAX_VALUE, function(m, n) {
        if(m < n) return m;
        else return n;
    });
}
```

```js
// return the largest number in the array
// (or Number.MIN_VALUE if the array is empty)
function max(numbers) {
    return reduce(numbers, Number.MIN_VALUE, function(m, n) {
        if(m > n) return m;
        else return n;
    });
}
```

### Things you can do with `reduce()`

* *Undo/redo*. Current state is a list of user interactions, undo just means removing the
last interaction from the list.

* *Replaying user interaction for testing*. Initial value is the initial state of the
system, and your array is a sequence of user interactions.

* *Time-traveling debugger*. Examine the state at any point in time, fix the problem,
then play it forward with the new code.

* *Audit trails*. Get the state of the system at a certain point in time.

### Implementation of `map()` and `filter()` using `reduce()`

`map()` implemetations:

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

`filter()` implemetations:

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

### Summary

* The three most common functional tools are `map()`, `filter()`, and `reduce()`. Nearly
every functional programmer uses them often.

* `map()`, `filter()`, and `reduce()` are essentially specialized for loops over arrays. They
can replace those for loops and add clarity because they are special-purpose.

* `map()` transforms an array into a new array. Each element is transformed with the
callback you specify.

* `filter()` selects a subset of elements from one array into a new array. You choose which
elements are selected by passing in a predicate.

* `reduce()` combines elements of an array, along with an initial value, into a single value.
It is used to summarize data or to build a value from a sequence.

## Chapter 13. Chaining functional tools

### Vocab

The *identity function* is a function that returns its argument unchanged. It appears to do
nothing, but it is useful for indicating just that: Nothing should be done.

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

Функция `max()`, для поиска максимального значения, с использованием `>` (была ранее):

```js
function max(numbers) {
    return reduce(numbers, Number.MIN_VALUE, function(m, n) {
        if(m > n) return m;
        else return n;
    });
}
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

Original:

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

// (1) higher-order functions are called in named functions to add context.
function selectBestCustomers(customers) {               // (1)
    return filter(customers, function(customer) {
        return customer.purchases.length >= 3;
    });
}

// (2) getBiggestPurchase. We can extract this higher-order function as well.
function getBiggestPurchases(customers) {               // (1)
    return map(customers, getBiggestPurchase);          // (2)
}

function getBiggestPurchase(customer) {                 // (2)
    return maxKey(customer.purchases, {total: 0}, function(purchase) {
        return purchase.total;
    });
}
```

### Clarifying chains, method 2: Naming the callbacks

Original:

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
var prices = pluck(products, ‘price’);

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
> console.log(howMany[‘ties’])      // 4
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

### Where to find functional tools

* Lodash: Functional tools for JavaScript

    [Lodash documentation (https://lodash.com/docs)](https://lodash.com/docs)

* Laravel Collections • Functional tools for PHP

    [Laravel collections documentation: (https://laravel.com/docs/collections#available-methods)](https://laravel.com/docs/collections#available-methods)

* Clojure standard library

    [ClojureDocs quick reference: (https://clojuredocs.org/quickref#sequences)](https://clojuredocs.org/quickref#sequences)

    [Official docs: (https://clojure.github.io/clojure/clojure.core-api.html)](https://clojure.github.io/clojure/clojure.core-api.html)

* Haskell Prelude

    [Haskell Prelude: (http://www.cse.chalmers.se/edu/course/TDA555/tourofprelude.html)](http://www.cse.chalmers.se/edu/course/TDA555/tourofprelude.html)

### JavaScript conveniences (удобства)

В JavaScript уже есть встроенные функции наподобие `map()`, `filter()` и `reduce()`:

```js
// В этой книге
var customerNames = map(customers, function(c) {
    return c.firstName + " " + c.lastName;
});

// Встроенная в JavaScript
var customerNames = customers.map(function(c) {
    return c.firstName + " " + c.lastName;
});
```

В JavaScript есть облегченный синтакс для inline функций ('=>'):

```js
var window = 5;
var answer =
    range(0, array.length)
        .map(i => array.slice(i, i + window))
        .map(average);
```

А еще в JavaScript можно сделать так:

```js
var window = 5;
var average = array => array.reduce((sum, e) => sum + e, 0) / array.length;
var answer = array.map((e, i) => array.slice(i, i + window)).map(average);
```

### Примеры functional tool chaining для разных языков программирования

ES6:

```js
function movingAverage(numbers) {
    return numbers
        .map((_e, i) => numbers.slice(i, i + window))
        .map(average);
}
```

Classic JavaScript with Lodash:

```js
function movingAverage(numbers) {
    return _.chain(numbers)
        .map(function(_e, i) { return numbers.slice(i, i + window); })
        .map(average)
        .value();
}
```

Java 8 Streams:

```java
public static double average(List<Double> numbers) {
    return numbers
        .stream()
        .reduce(0.0, Double::sum) / numbers.size();
}

public static List<Double> movingAverage(List<Double> numbers) {
    return IntStream
        .range(0, numbers.size())
        .mapToObj(i -> numbers.subList(i, Math.min(i + 3, numbers.size())))
        .map(Utils::average)
        .collect(Collectors.toList());
}
```

```csharp
public static IEnumerable<Double> movingAverage(IEnumerable<Double> numbers) {
    return Enumerable
        .Range(0, numbers.Count())
        .Select(i => numbers.ToList().GetRange(i, Math.Min(3, numbers.Count() - i)))
        .Select(l => l.Average());
}
```

### Summary

* We can combine functional tools into multi-tep chains. Their combination allows us to
express very complex computations over data in small, clear steps.

* One perspective of chaining is that the functional tools form a query language, much like
SQL. Chaining functional tools lets you express complex queries over arrays of data.

* We often have to make new data or augment existing data to make subsequent steps
possible. Look for ways of representing implicit information as explicit data.

* There are many functional tools. You will find them as you refactor your code. You can
also find inspiration for them in other languages.

* Functional tools are making their way into languages that are traditionally not
considered functional, like Java. Use them where they are appropriate (подходят).
