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



Остановился тут, на странице 259

### Refactoring: Replace body with callback

1. Identify the before, body, and after sections.
2. Extract the whole thing into a function.
3. Extract the body section into a function passed as an argument to that function.

#### Examples of non-first-class things in JavaScript

*(Примеры сущностей/конструкций, которые нельзя использовать для передачи в качестве входных и выходных параметров в функциях)*

1. Arithmetic operators
2. For loops
3. If statements
4. Try/catch blocks

#### Examples of things you can do with a first-class value

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
