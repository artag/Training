# Grokking Simplicity

## Part 2: First-class abstractions

## Chapter 14. Functional tools for nested data

### Vocab

The sequence of keys for locating a value in nested objects is called a *path*. The
path has one key for each level of nesting.

A *recursive function* is a function that is defined in terms of itself. A recursive
function will have a recursive call where the function calls itself.

A *base case* in recursion is a case with no recursive call that stops the recursion.
Each recursive call should make progress toward (в направлении) the base case.

### Deriving (получение) `update()`

Original function with smell:

```js
function incrementQuantity(item) {
    var quantity = item['quantity'];
    var newQuantity = quantity + 1;
    var newItem = objectSet(item, 'quantity', newQuantity);
    return newItem;
}
```

**Step 1.** Make field explicit argument:

```js
function incrementField(item, field) {
    var value = item[field];
    var newValue = value + 1;
    var newItem = objectSet(item, field, newValue);
    return newItem;
}
```

**Step 2.** Extract into its own function:

```js
function incrementField(item, field) {
    return updateField(item, field, function(value) {   // pass in modify function
        return value + 1;
    });
}

function updateField(item, field, modify) {
    var value = item[field];
    var newValue = modify(value);
    var newItem = objectSet(item, field, newValue);
    return newItem;
}
```

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

### Deriving (получение) `update2()`

Original function:

```js
function incrementSize(item) {
    var options = item.options;                                 // get 1
    var size = options.size;                                    // get 2
    var newSize = size + 1;                                     // modify
    var newOptions = objectSet(options, 'size', newSize);       // set 1
    var newItem = objectSet(item, 'options', newOptions);       // set 2
    return newItem;
}
```

**Step 1.** get, modify, set to replace with `update()`:

```js
function incrementSize(item) {
    var options = item.options;                                 // get 1
    var newOptions = update(options, 'size', increment);        // modify
    var newItem = objectSet(item, 'options', newOptions);       // set 1
    return newItem;
}
```

Refactored twice:

```js
function incrementSize(item) {
    return update(item, 'options', function(options) {
        return update(options, 'size', increment);
    });
}
```

**Step 2.** Make arguments explicit:

```js
function incrementOption(item, option) {
    return update(item, 'options', function(options) {
        return update(options, option, increment);          // 'size' -> option
    });
}
```

and another one:

```js
function updateOption(item, option, modify) {
    return update(item, 'options', function(options) {
        return update(options, option, modify);             // increment -> modify
    });
}
```

**Step 3.** Make third argument explicit:

```js
// (1) - key1, make this an explicit argument
function update2(object, key1, key2, modify) {          // (1)
    return update(object, key1, function(value1) {      // (1)
        return update(value1, key2, modify);
    });
}
```

Using `update2()`:

```js
function incrementSize(item) {
    return update2(item, 'options', 'size', function(size) {
        return size + 1;
    });
}
```

### Functional tool: `update2()`

`update2` - the 2 means nested twice. Modify a value nested twice within objects.

```js
function update2(object, key1, key2, modify) {
    return update(object, key1, function(value1) {
        return update(value1, key2, modify);
    });
}
```

Похожим образом получаются остальные функции `update3()`, `update4()`, ...

### Functional tool: `update3()`

```js
function update3(object, key1, key2, key3, modify) {
    return update(object, key1, function(object2) {
        return update2(object2, key2, key3, modify);
    });
}

function update4(object, k1, k2, k3, k4, modify) {
    return update(object, k1, function(object2) {
        return update3(object2, k2, k3, k4, modify);
    });
}

function update5(object, k1, k2, k3, k4, k5, modify) {
    return update(object, k1, function(object2) {
        return update4(object2, k2, k3, k4, k5, modify);
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

### Summary

* `update()` is a functional tool that implements a common pattern. It lets you modify a
value inside of an object without manually pulling the value out and setting it back in.

* `nestedUpdate()` is a functional tool that operates on deeply nested data. It is very
useful for modifying a value when you know the path of keys to where it is located.

* Iteration (loops) can often be clearer than recursion. But recursion is clearer and easier
when operating on nested data.

* Recursion can use the function call stack to keep track of where it left off before calling
itself. This lets a recursive function's structure mirror the structure of nested data.

* Deep nesting can lead to difficulty of understanding. When you operate on deeply nested
data, you often have to remember all of the data structures and their keys along the path.

* You can apply abstraction barriers to key data structures so that you don't have as much
to remember. This can make working with deep structures easier.
