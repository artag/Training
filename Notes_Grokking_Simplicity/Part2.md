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

1. Copy-on-write discipline for arrays.

```js
function withArrayCopy(array, modify) {
    var copy = array.slice();
    modify(copy);
    return copy;
}
```

2. Copy-on-write discipline for objects.

```js
function withObjectCopy(object, modify) {
    var copy = Object.assign({}, object);
    modify(copy);
    return copy;
}

// Usage 1
function objectSet(object, key, value) {
    return withObjectCopy(object, function(copy) {
        copy[key] = value;
    });
}

// Usage 2
function objectDelete(object, key) {
    return withObjectCopy(object, function(copy) {
        delete copy[key];
    });
}
```

3. Try/catch

```js
function tryCatch(f, errorHandler) {
    try {
        return f();
    } catch(error) {
        return errorHandler(error);
    }
}
```

4. When

```js
function when(test, then) {
    if(test)
        return then();
}
```

5. If

```js
function IF(test, then, ELSE) {
    if(test)
        return then();
    else
        return ELSE();
}
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
