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

## Chapter 15. Isolating timelines

### Vocab

A *timeline* is a sequence of actions over time. There can be multiple timelines
running at the same time in your system.

Multiple timelines can run in different ways, depending on timing. The ways that
timelines can run are known as *possible orderings*. A single timeline has one
possible ordering.

Actions on different timelines may *interleave* if they can occur between each
other. This happens when multiple threads run at the same time.

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

For any kind of input/output you use an asynchronous model. This means that you give
it a callback that will be called with the result of the input/output operation.

Because the input/output operation can take an unknown amount of time, the callback
will be called at some uncontrollable, unknown time in the future.
That's why doing an asynchronous call creates a new timeline.

### Different languages, different threading models

* Single-threaded, synchronous. (example: PHP)
  * Everything happens in order.
  * When you do any kind of input/output, your whole program blocks while waiting for it
  to complete.
  * You can still have other timelines if you contact a different computer, like you
    would with an API. Those timelines can't share memory, so you eliminate a huge class
    of shared resources.

* Single-threaded, asynchronous. (example: JavaScript)
  * One thread.
  * Synchronous actions, like modifying a global variable, cannot be interleaved between
  timelines.
  * For any kind of input/output you can use an asynchronous model.
  * Asynchronous means that you give it a callback.
  * The callback  will be called at some uncontrollable, unknown time in the future:
  an asynchronous call creates a new timeline.

* Multi-threaded. (example: Java, Python, Ruby, C, C#)
  * Is the most difficult to program.
  * Every new thread creates a new timeline.
  * You need to use constructs like locks.

* Message-passing processes. (example: Erlang, Elixir)
  * Many different processes can be run simultaneously.
  * Each process is a separate timeline.
  * Unique thing - that processes choose which message they will process next.
  * The actions of individual timelines do interleave, but because they don’t share any
  memory, they usually don’t share resources. You don’t have to worry about the large
  number of possible orderings.

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

Formula for number of possible orderings:

```text
     (t*a)!
o = -------
     (a!)^t
```

`o` - possible orederings

`t` - # of timelines

`a` - # of actions per timeline

`!` - is factorial

2. Shorter timelines are easier.

3. Sharing fewer (меньшее число) resources is easier.

4. Coordinate when resources are shared.

Иногда от общих ресурсов невозможно избавиться. Поэтому необходима координация их использования
между различными timeline'ами.

5. Manipulate time as a first-class concept (создание повторно используемых объектов
для манипуляций с timeline).

The ordering and proper timing of actions is difficult. We can make this easier by
creating reusable objects that manipulate the timeline.

### Summary

* Timelines are sequences of actions that can run simultaneously. They capture what code
runs in sequence and what runs in parallel.

* Modern software often runs with multiple timelines. Each computer, thread, process, or
asynchronous callback adds a timeline.

* Because actions on timelines can interleave in ways we can't control, multiple timelines
result in many different possible orderings. The more orderings, the harder it is to
understand whether your code will always lead to the right result.

* Timeline diagrams can show how our code runs sequentially and in parallel. We use the
timeline diagrams to understand where they can interfere with each other.

* It's important to understand your language's and platform's threading model. For
distributed systems, understanding how things run sequentially and in parallel in your
system is key.

* Shared resources are a source of bugs. By identifying and removing resources, we make
our code work better.

* Timelines that don't share resources can be understood and executed in isolation.
Это упрощает понимание, написание и поддержку кода.
