# Grokking Simplicity

## Part 2: First-class abstractions

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
