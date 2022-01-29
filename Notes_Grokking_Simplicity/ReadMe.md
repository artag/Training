# Grokking Simplicity

* [Part 1: Actions, calculations, and data](Part1.md)

* [Part 2: First-class abstractions](Part2.md)

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

2. Функция, которая создает "сумматор":

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
