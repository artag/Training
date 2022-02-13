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

* Identify actions
* Draw actions
  * Actions that execute in sequence - one after the other
  * Actions that execute in parallel - simultaneous, left first, or right first
    * Asynchronous callbacks
    * Multiple threads
    * Multiple processes
    * Multiple machines
* Simplify the timeline
  * If two actions cannot be interleaved, combine them into a single box.
  * If one timeline ends and starts another, consolidate them into a single timeline.
  * Add dotted lines when order is constrained.
* Reading timelines
  * Actions in different timelines can occur in orders:
    * simultaneous
    * left first
    * right first
  * Evaluate (оцените) the orders as impossible, desirable, or undesirable (нежелательный).

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
  * The actions of individual timelines do interleave, but because they don't share any
  memory, they usually don't share resources. You don't have to worry about the large
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

### Simplifying the timeline

1. Consolidate all actions on a single timeline.

2. Consolidate timelines that end by creating one new timeline.

### Timelines that share resources can cause problems

We can remove problems by not sharing resources

#### Converting a global variable to a local one

1. Identify the global variable we would like to make local.

2. Replace the global variable with a local variable.

Example:

```js
// Identify the global variable we would like to make local
function calc_cart_total() {
    total = 0;                          // total - global variable
    cost_ajax(cart, function(cost) {
        total += cost;                  // read - modify - write global variable
        shipping_ajax(cart, function(shipping) {
            total += shipping;
            update_total_dom(total);
        });
    });
}
```

```js
// Replace the global variable with a local variable
function calc_cart_total() {
    var total = 0;                      // use a local variable instead
    cost_ajax(cart, function(cost) {
        total += cost;
        shipping_ajax(cart, function(shipping) {
            total += shipping;
            update_total_dom(total);
        });
    });
}
```

#### Converting a global variable to an argument

1. Identify the implicit (неявный) input.

2. Replace the implicit (неявный) input with an argument.

Example:

```js
// Identify the implicit input
function add_item_to_cart(name, price, quantity) {
    cart = add_item(cart, name, price, quantity);
    calc_cart_total();
}
function calc_cart_total() {
    var total = 0;
    cost_ajax(cart, function(cost) {                // cart can be changed between reads
        total += cost;
        shipping_ajax(cart, function(shipping) {    // cart can be changed between reads
            total += shipping;
            update_total_dom(total);
        });
    });
}
```

```js
// Replace the implicit input with an argument
function add_item_to_cart(name, price, quantity) {
    cart = add_item(cart, name, price, quantity);
    calc_cart_total(cart);                          // add the cart as an argument
}
function calc_cart_total(cart) {                    // add the cart as an argument
    var total = 0;
    cost_ajax(cart, function(cost) {                // read is not to global variable anymore
        total += cost;
        shipping_ajax(cart, function(shipping) {    // read is not to global variable anymore
            total += shipping;
            update_total_dom(total);
        });
    });
}
```

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

## Chapter 16. Sharing resources between timelines

### Vocab

A *queue* is a data structure where items are removed in the same order they are added.

A *concurrency primitive* is a piece of reusable functionality that helps share resources
across timelines.

### Building a queue in JavaScript

A queue is a data structure. We can use it to coordinate timelines. We call it a
*concurrency primitive*. It's a small piece of reusable functionality that helps share
resources.

**Step 1**. Create simple queue.

```js
var queue_items = [];       // array to store items

function update_total_queue(cart) {
    queue_items.push(cart);
}
```

**Step 2**. Do the work on the first item in the queue.

```js
var queue_items = [];

function runNext() {
    var cart = queue_items.shift();             // pull the first item off the array
    calc_cart_total(cart, update_total_dom);
}
function update_total_queue(cart) {
    queue_items.push(cart);
    setTimeout(runNext, 0);         // adds a job to the JavaScript event loop
}
```

**Step 3**. Prevent a second timeline from running at the same time as the first.

```js
var queue_items = [];
var working = false;        // busy flag

function runNext() {
    if(working)             // prevent two from running at the same time
        return;
    working = true;
    var cart = queue_items.shift();
    calc_cart_total(cart, update_total_dom);
}
function update_total_queue(cart) {
    queue_items.push(cart);
    setTimeout(runNext, 0);
}
```

**Step 4**. Modify the callback to `calc_cart_total()` to start the next item.

```js
var queue_items = [];
var working = false;

function runNext() {
    if(working)
        return;
    working = true;
    var cart = queue_items.shift();
    calc_cart_total(cart, function(total) {     // done working and start the next item
        update_total_dom(total);
        working = false;
        runNext();
    });
}
function update_total_queue(cart) {
    queue_items.push(cart);
    setTimeout(runNext, 0);
}
```

**Step 5**. Stop going through items when there are no more

```js
var queue_items = [];
var working = false;

function runNext() {
    if(working)
        return;
    if(queue_items.length === 0)            // stop if we have no items left
        return;
    working = true;
    var cart = queue_items.shift();
    calc_cart_total(cart, function(total) {
        update_total_dom(total);
        working = false;
        runNext();
    });
}
function update_total_queue(cart) {
    queue_items.push(cart);
    setTimeout(runNext, 0);
}
```

**Step 6**. Wrap the variables and functions in a function scope.

```js
function Queue() {
    var queue_items = [];
    var working = false;

    function runNext() {
        if(working)
            return;
        if(queue_items.length === 0)
            return;
        working = true;
        var cart = queue_items.shift();
        calc_cart_total(cart, function(total) {
            update_total_dom(total);
            working = false;
            runNext();
        });
    }
    return function(cart) {     // Queue() returns the function, which adds to the queue
        queue_items.push(cart);
        setTimeout(runNext, 0);
    };
}
```

We can run the returned function just like before:

```js
var update_total_queue = Queue();
```

**Step 7**. Making the queue reusable. Extracting the `done()` function.

`done()` is a callback that continues the work of the queue timeline. It sets working to
`false` so that the next time through, it won't return early. Then it calls `runNext()` to
initiate the next iteration.

```js
function Queue() {
    var queue_items = [];
    var working = false;

    function runNext() {
        if(working)
            return;
        if(queue_items.length === 0)
            return;
        working = true;
        var cart = queue_items.shift();
        function worker(cart, done) {   // extract cart local to argument as well
            calc_cart_total(cart, function(total) {
                update_total_dom(total);
                done(total);            // done is the name of the callback
            });
        }
        worker(cart, function() {       // extract two lines into a new function
            working = false;
            runNext();
        });
    }

    return function(cart) {
        queue_items.push(cart);
        setTimeout(runNext, 0);
    };
}

var update_total_queue = Queue(calc_cart_worker);
```

**Step 8**. Extracting the custom worker behavior. Generic `Queue`.

```js
function Queue(worker) {    // add a new argument, the function that does the work
    var queue_items = [];
    var working = false;

    function runNext() {
        if(working)
            return;
        if(queue_items.length === 0)
            return;
        working = true;
        var cart = queue_items.shift();

        worker(cart, function() {
            working = false;
            runNext();
        });
    }

    return function(cart) {
        queue_items.push(cart);
        setTimeout(runNext, 0);
    };
}

function calc_cart_worker(cart, done) {         // extracted function
    calc_cart_total(cart, function(total) {
        update_total_dom(total);
        done(total);
    });
}

var update_total_queue = Queue(calc_cart_worker);
```

**Step 9**. Accepting a callback for when the task is complete.

Add one more feature, which is the ability to pass in a callback that will be called
when our task is done.

```js
// store both the data for the task and the callback in a small object.
// (1) - item.data. Pass the worker just the data.
// (2) - push both the data and the callback onto the array.
function Queue(worker) {
    var queue_items = [];
    var working = false;

    function runNext() {
        if(working)
            return;
        if(queue_items.length === 0)
            return;
        working = true;
        var item = queue_items.shift();
        worker(item.data, function() {              // (1)
            working = false;
            runNext();
        });
    }

    return function(data, callback) {               // (2)
        queue_items.push({
            data: data,                             // (2)
            callback: callback || function(){}      // (2)
        });                                         // (2)
        setTimeout(runNext, 0);
    };
}

function calc_cart_worker(cart, done) {
    calc_cart_total(cart, function(total) {
        update_total_dom(total);
        done(total);
    });
}

var update_total_queue = Queue(calc_cart_worker);
```

`callback || function(){}` - if callback is undefined, use a function that does
nothing instead.

**Step 10**. Calling the callback when the task is complete.

```js
// (1) - Queue() is very generic, so the variable names are generic as well
// (2) - we allow done() to accept an argument.
// (3) - set up asynchronous call to item.callback.
// (4) - cart will get the item data; we call done() when we're done.
// (5) - here we know the specifics of what we're doing, so we use specific variable names.
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

function calc_cart_worker(cart, done) {         // (4)
    calc_cart_total(cart, function(total) {     // (5)
        update_total_dom(total);
        done(total);
    });
}

var update_total_queue = Queue(calc_cart_worker);
```

### `DroppingQueue`

Выполняет только ограниченное число/(последний) action из всех action в очереди.

*Мое примечание: программный аналог бездребезговой кнопки.*

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

### Summary

* Timing issues are hard to reproduce and often pass our tests. Use timeline diagrams to
analyze and diagnose timing issues.

* When you have a resource-sharing bug, look to the real world for inspiration for how to
solve it. People share stuff all the time, very often with no problems. Learn from people.

* Build reusable tools that help you share resources. They are called
*concurrency primitives*, and they make your code clearer and simpler.

* Concurrency primitives often take the form of higher-order functions on actions. Those
higher-order functions give the actions superpowers.

* Concurrency primitives don't have to be difficult to write yourself. Take small steps and
refactor and you can build your own.

## Chapter 17. Coordinating timelines

Sometimes, multiple timelines need to work together when there's no explicit resource they
are sharing. In this chapter, we will build a concurrency primitive that will help
timelines coordinate and eliminate incorrect possible orderings.

### Vocab

A *race condition* occurs when the behavior depends on which timeline finishes first.

An action that only has an effect the first time you call it is called idempotent.
`JustOnce()` makes any action idempotent.

`Идемпотентность` - свойство объекта или операции при повторном применении операции к
объекту давать тот же результат, что и при первом.

Примеры идемпотентных операций:

* сложение с нулем `a = a + 0`
* умножение на единицу `x = x * 1`
* модуль числа: `x = |x|`

### Waiting for both parallel callbacks

Our goal: We want the ajax responses to come back in parallel, but we want to wait for
both of them before writing to the DOM.

```text
Read total          Read total
Write total         Write total
    |                    |
           Read total
        update_total_dom()
```

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

Using:

Before:

```js
function calc_cart_total(cart, callback) {
    var total = 0;

    // timeline 1
    cost_ajax(cart, function(cost) {
        total += cost;
    });

    // timeline 2
    shipping_ajax(cart, function(shipping) {
        total += shipping;
        callback(total);
    });
}
```

With `Cut()`:

```js
function calc_cart_total(cart, callback) {
    var total = 0;
    var done = Cut(2, function() {
        callback(total);
    });

    // timeline 1
    cost_ajax(cart, function(cost) {
        total += cost;
        done();
    });

    // timeline 2
    shipping_ajax(cart, function(shipping) {
        total += shipping;
        done();
    });
}
```

#### 1. What scope (область) to store `Cut()`

We need to call `done()` at the end of each callback. That suggests we create the
`Cut()` in the scope of `calc_cart_total()`, where both callbacks are created.

#### 2. What the callback for `Cut()` is

Inside of `calc_cart_total()`, we have already separated the callback that should happen
after the total is calculated. We'll just pass that callback through to `Cut()`.

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

Usage:

```js
var sendAddToCartTextOnce = JustOnce(sendAddToCartText);

sendAddToCartTextOnce("555-555-5555-55");       // only the first one sends a text
sendAddToCartTextOnce("555-555-5555-55");
```

JavaScript has one thread. Other languages would need to use locks to share a mutable
variable.

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

### Summary

* Functional programmers build a new model of time on top of the implicit model
provided by the language. The new model has properties that help them solve the
problem they are working on.

* The explicit model of time is often built with first-class values. Being first-class means
you have the whole language available to manipulate time.

* We can build concurrency primitives that coordinate two timelines. Those primitives
constrain the possible orderings, helping us ensure the correct result is achieved every
time.

* Cutting timelines is one way to coordinate between timelines. Cutting allows multiple
timelines to wait for all timelines to finish before one continues.

