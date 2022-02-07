# Grokking Simplicity

## Part 1: Actions, calculations, and data

## Chapter 1 and 2

Common side effects include:

* Sending an email
* Reading a file
* Blinking a light
* Making a web request
* Applying the brakes in a car

*Side effects* are any behavior of a function besides the return value.

*Pure functions* depend only on their arguments and don't have any side effects.

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

### Actions, calculations, and data

Functional programmers distinguish between actions, calculations, and data (ACD).

1. Actions

- Depend on how many times or when it is run.

- **Also called** *functions with side-effects*, *side-effecting functions*, *impure functions*

- **Examples**: Send an email, read from a database

Anything that depends on when it is run, or how many times it is run, or both, is an *action*.
If I send an urgent email today, it's much different from sending it next week. And of course,
sending the same email 10 times is different from sending it 0 times or 1 time.

2. Calculations

- Computations from input to output.

- **Also called** *pure functions*, *mathematical functions*

- **Examples**: Find the maximum number, check if an email address is valid

*Calculations* are computations from input to output. They always give the same output when you
give them the same input. You can call them anytime, anywhere, and it won't affect anything
outside of them. That makes them really easy to test and safe to use without worrying about
how many times or when they are called.

3. Data

- Facts about events

- **Examples**: The email address a user gave us, the dollar amount read from a bank's API.

*Data* is recorded facts about events. We distinguish data becauseit is not as complex as
executable code. It has well-understood properties. Data is interesting because it is meaningful
without being run. It can be interpreted in multiple ways. Take a restaurant receipt as
an example: It can be used by the restaurant manager to determine which food items are popular.
And it can be used by the customer to track their dining-out budget.

Calculations are *referentially transparent* because a call to a calculation can be replaced by
its result. For instance, `+` is a calculation. `2 + 3` always results in `5`, so you could
replace the code `2 + 3` with `5` and have an equivalent program.

1. We can apply the ACD perspective to any situation
2. Actions can hide actions, calculations, and data
3. Calculations can be composed of smaller calculations and data
4. Data can only be composed of more data
5. Calculations often happen "in our heads"

### Summary

* Actions, calculations, and data are the first and most important distinction functional
programmers make. We need to learn to see them in all code we read. We'll start to apply
this distinction in chapter 3.
* Functional programmers apply stratified design to make their code easier to maintain.
The layers help organize code by rate of change. We see the exact process of stratified
design in chapters 8 and 9.
* Timeline diagrams can help you visualize how actions will run over time. They can help
you see where actions will interfere with each other. We'll learn the process of drawing
timeline diagrams in chapter 15.
* We learned to cut timelines to coordinate their actions. Coordinating them helps us
guarantee that they perform their actions in the proper order. We'll see exactly how to
cut timelines in chapter 17 in a very similar scenario to the pizza kitchen.

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

### Deep dive: Data

**What is data?**

Data is facts about events. It is a record of something that happened. Functional programmers
tap into the rich tradition of record-­keeping that started thousands of years ago.

**How do we implement data?**

Data is implemented in JavaScript using the built-in data types. These include numbers,
strings, arrays, and objects, among others. Other languages have more sophisticated ways of
implementing data. For instance, Haskell lets you define new data types that encode the
important structure of your domain.

**How does data encode meaning?**

Data encodes meaning in structure. The structure of your data should mirror the structure of
the problem domain you are implementing. For instance, if the order of a list of names is
important, you should choose a data structure that maintains order.

**Immutability**

Functional programmers use two main disciplines for implementing immutable data:

1. *Copy-on-write*. Make a copy of data before you modify it.
2. *Defensive copying*. Make a copy of data you want to keep.
We will learn about these disciplines in chapters 6 and 7.

**Examples**

* A list of foods to buy
* Your first name
* My phone number
* A receipt for a meal

**What are the advantages of data?**

Data is useful mostly because of what it can't do. Unlike actions and calculations, it cannot be
run. It is inert. That is what lets it be so well understood.

1. *Serializable*. Actions and calculations have trouble being serialized to be run on another
machine without a lot of trouble. Data, however, has no problem being transmitted
over a wire or stored to disk and read back later. Well-preserved data can last for
thousands of years. Will your data last that long? I can't say. But it sure will last longer
than code for a function.

2. *Compare for equality*. You can easily compare two pieces of data to see if they are equal. This
is impossible for calculations and actions.

3. *Open for interpretation*. Data can be interpreted in multiple ways. Server access
logs can be mined to debug problems. But they can also be used to know where your
traffic is coming from. Both use the same data, with different interpretations.

**Disadvantages**

Interpretation is a double-edged sword. Although it's an advantage that data can be
interpreted in different ways, it is a disadvantage that data must be interpreted to be useful.
A calculation can run and be useful, even if we don't understand it. But data needs a machine to
interpret it. Data has no meaning without interpretation. It's just bytes.

### Deep dive: Calculations

**What are calculations?**

Calculations are computations from inputs to outputs. No matter when they are run, or how
many times they are run, they will give the same output for the same inputs.

**How do we implement calculations?**

We typically represent calculations as functions. That's what we do in JavaScript. In languages
without functions, we would have to use something else, like a class with a method.

**How do calculations encode meaning?**

Calculations encode meaning as computation. A calculation represents some computation from
inputs to outputs. When or how you use it depends on whether that calculation is appropriate
for the situation.

**Why prefer calculations to actions?**

Calculations have benefits compared to actions:

1. They're much easier to test. You can run them as many times as you want or wherever you
want (local machine, build server, testing machine) in order to test them.

2. They're easier to analyze by a machine. A lot of academic research has gone into what's
called “static analysis.” It's essentially automated checks that your code makes
sense. We won't get into that in this book.

3. They're very composable. Calculations can be put together into bigger calculations in very
flexible ways. They can also be used in what are called “higher-order” calculations. We'll
get to those in chapter 14.

**Examples of calculations**

* Addition and multiplication
* String concatenation
* Planning a shopping trip

**What worries do calculations avoid?**

Functional programmers prefer using calculations instead of actions when possible because
calculations are so much easier to understand. You can read the code and know what it is going
to do. There's a whole list of things you don't have to worry about:

1. What else is running at the same time
2. What has run in the past and what will run in the future
3. How many times you have already run it

**Disadvantage**

Calculations do have their downside, which they share with actions. You can't really know what
calculations or actions are going to do without running them.
Of course, you, the programmer, can read the code and sometimes see what it will do. But as
far as your running software is concerned, a function is a black box. You give it some inputs
and an output comes out. You can't really do much else with a function except run it.
If you can't live with this disadvantage and you need something understandable, you must
use data instead of calculations or actions.

**What are they typically called?**

Outside of this book, calculations are typically called *pure functions* or
*mathematical functions*. We call them *calculations* in Grokking Simplicity to avoid ambiguities
with specific language features such as JavaScript functions.

### Deep dive: Actions

**What are actions?**

Actions are anything that have an effect on the world or are affected by the world. As a
rule of thumb, actions depend on when or how many times they are run.

* When they are run - *Ordering*
* How many times they are run - *Repetition*

**How are actions implemented?**

In JavaScript, we use functions to implement actions. It is unfortunate that we use the same
construct for both actions and calculations. That can get confusing. However, it is some-
thing you can learn to work with.

**How do actions encode meaning?**

The meaning of an action is the effect it has on the world. We should make sure the effect
it has is the one we want.

**Examples**

* Sending an email
* Withdrawing money from an account
* Modifying a global variable
* Sending an ajax request

**What are they typically called?**

Outside of this book, actions are typically called *impure functions*, *side-effecting functions*,
or *functions with side effects*. We call them actions in Grokking Simplicity to avoid
ambiguities with specific language features such as JavaScript functions.

**Actions pose a tough bargain**

A. They are a pain to deal with.

B. They are the reason we run our software in the first place.

That's one nasty bargain, if you ask me. But it's what we have to live with, regardless of the
paradigm we work in. Functional programmers accept the bargain, and they have a bag
of tricks for how to best deal with actions. We can take a look at a few of the tricks:

1. Use fewer actions if possible. We can never get all the way down to zero actions, but if
an action isn't required, use a calculation instead. We look at that in chapter 15.

2. Keep your actions small. Remove everything that isn't necessary from the
action. For instance, you can extract a planning stage, implemented as a
calculation, from the execution stage, where the necessary action is carried out.
We explore this technique in the next chapter.

3. Restrict your actions to interactions with the outside. Your actions are all of those
things that are affected by the world outside or can affect the world outside.
Inside, ideally, is just calculations and data. We'll see this more when we talk
about the onion architecture in chapter 18.

4. Limit how dependent on time an action is. Functional programmers have techniques
for making actions a little less difficult to work with. These techniques include
making actions less dependent on when they happen and how many times they
are run.

### Actions can take many forms

* Function calls

```js
// making this little pop-up appear is an action
alert("Hello world!");
```

* Method calls

```js
// prints to the console
console.log("hello");
```

* Constructors

```js
// makes a different value depending on when you call it.
new Date()
```

* Expressions

```js
// if y is a shared, mutable variable, reading it can be different at different times
y                   // variable access

// if user is a shared, mutable object, reading first_name could be different each time
user.first_name     // property access

// if stack is a shared, mutable array, the first element could be different each time
stack[0]            // array access
```

* Statements

```js
// writing to a shared, mutable variable is an action because it can affect other parts of the code
z = 3;                     // assigment

// deleting a property can affect other parts of the code, so this is an action
delete user.first_name;    // property deletion
```

### Summary

* Functional programmers distinguish three categories: actions, calculations, and data.
Learning this distinction is your first task as a functional programmer.
* Actions are things that depend on when or how many times they are run. Usually, these
are things that affect the world or are affected by the world.
* Calculations are computations from inputs to outputs. They don't affect anything outside
of themselves, and hence it doesn't matter when or how many times they are run.
* Data is facts about events. We record the facts immutably since the facts don't change.
* Functional programmers prefer data over calculations, and they prefer calculations over
actions.
* Calculations are easier to test than actions since calculations always return the same
answer for a given input.

## Chapter 4. Extracting calculations from actions

* Extract calculations from actions
* Copy-on-write
* Aligning design with business requirements
  * Choosing a better level of abstraction that matches usage

The *document object model* (DOM) is the in-memory representation of an HTML page in a browser.

*Actions spread*. We only have to identify one action inside a function for the whole function
to be an action.

Information enters a function through the inputs. Information and effects leave a function
through the outputs.

Functional programmers call these implicit inputs and outputs *side effects*.
They are not the main effect of the function (which is to calculate a return value).

*Explicit inputs* - Arguments.

*Implicit inputs* - Any other input

*Explicit outputs* - Return value

*Implicit outputs* - Any other output

Assigning a global variable is an *output* because data is leaving the function.

Reading a global variable is an *input* because data is entering the function.

```js
function calc_total() {
    shopping_cart_total = 0;                            // output
    for(var i = 0; i < shopping_cart.length; i++) {     // input
        var item = shopping_cart[i];
        shopping_cart_total += item.price;              // output
    }
}
```

Copying a mutable value before you modify it is a way to implement immutability.
It's called *copy-on-write*. We'll get into the details in chapter 6.

```js
function add_item(cart, name, price) {
    var new_cart = cart.slice();    // make a copy and assign it to local variable
    new_cart.push({                 // modify copy
        name: name,
        price: price
    });
    return new_cart;                // return copy
}
```

### Step-by-step: Extracting a calculation

Extracting a calculation from an action is a repeatable process. Here are the steps.

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

It's important to note here that we want our arguments and return values to be
immutable values—that is, they don't change. If we return a value and some piece of our
function later changes it, that's a kind of implicit output. Similarly, if something changes
the argument values after our function has received them, that is a kind of implicit input.
We'll learn way more about immutable data in chapter 6, including why we use it and how
to enforce it. For now, let's just assume we don't change them.

### Summary

* Functions that are actions will have implicit inputs and outputs.
* Calculations have no implicit inputs or outputs by definition.
* Shared variables (such as globals) are common implicit inputs and outputs.
* Implicit inputs can often be replaced by arguments.
* Implicit outputs can often be replaced by return values.
* As we apply functional principles, we'll find the ratio of code in actions to code in
calculations shifting toward calculations.

## Chapter 5. Improving the design of actions

A *code smell* is a characteristic of a piece of code that might be a symptom of deeper problems.

### Principles

* Minimize (reducing) implicit inputs and outputs

* Categorizing our calculations. (By grouping our calculations, we learn something about layers
of meaning)

* Giving the code a once-over (беглый осмотр)

* Categorizing our calculations
  * By grouping our calculations, we learn something about layers of meaning

* Design is about pulling things apart
  * Easier to reuse
  * Easier to maintain
  * Easier to test

* Smaller functions and more calculations

### Principle: Design is about pulling things apart (разделение сущностей на части)

* Easier to reuse (Smaller, simpler functions are easier to reuse. They do less.
They make fewer assumptions.)

* Easier to maintain (Smaller functions are easier to understand and maintain. They have less code.
They are often obviously right (or wrong)).

* Easier to test (Smaller functions are easier to test. They do one thing, so you just test that
one thing).

### Действия в коде

* Improving the design by pulling `add_item()` apart
* Extracting a copy-on-write pattern
* Using `add_item()`
* Categorizing our calculations
* Smaller functions and more calculations

### Summary

* In general, we want to eliminate implicit inputs and outputs by replacing them with
arguments and return values.
* Design is about pulling things apart. They can always be put back together.
* As we pull things apart, and as functions have single responsibilities, we will find that
they are easy to organize around concepts.

## Chapter 6. Staying immutable in a mutable language

### Vocab

*Nested data*: Data structures inside data structures; we can talk about the inner data
structure and the top-level data structure.

*Shallow copy*: Copying only the top-level data structure in nested data.

*Structural sharing*: Two nested data structures referencing the same inner data structure

We say data is *deeply nested* when the nesting goes on for a while (продолжается некоторое время).
It's a relative term, but an example might be objects within objects within arrays within objects
within objects... The nesting can go on a long time.

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

```js
function add_element_last(array, elem) {    // we want to modify array
    var new_array = array.slice();          // 1. make a copy
    new_array.push(elem);                   // 2. modify the copy
    return new_array;                       // 3. return the copy
}

function remove_item_by_name(cart, name) {
    var new_cart = cart.slice();    // 1. make a copy
    var idx = null;
    for(var i = 0; i < new_cart.length; i++) {
        if(cart[i].name === name)
            idx = i;
    }
    if(idx !== null)
        new_cart.splice(idx, 1);    // 2. modify the copy
    return new_cart;                // 3. return the copy
}
```

* Copy-on-write converts writes into reads.

* These copy-on-write operations are generalizable

### What to do if an operation is a read and a write

Two approaches:

1. Split function (Splitting the operation into read and write).
2. Return two values.

Example. Remove from the front `.shift` This mutates the array by dropping the first element
(index 0) and returns the value that was dropped.

```js
> var array = [1, 2, 3, 4];
> array.shift()
1
> array
[2, 3, 4]
```

#### Splitting the operation into read and write

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

We can use them separately or together.

#### Returning two values from one function

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

или так (реализация с помощью функций из предыдущего раздела)

```js
function shift(array) {
    return {
        first : first_element(array),
        array : drop_first(array)
    };
}
```

### Reads to immutable data structures are calculations

* **Reads to mutable data are actions**

  If we read from a mutable value, we could get a different answer each time we read it,
  so reading mutable data is an action.

* **Writes make a given piece of data mutable**

  Writes modify data, so they are what make the data mutable.

* **If there are no writes to a piece of data, it is immutable**

  If we get rid of all of the writes by converting them to reads, the data won't ever change after
  it is created. That means it's immutable.

* **Reads to immutable data structures are calculations**

  Once we do make the data immutable, all of those reads become calculations.

* **Converting writes to reads makes more code calculations**

  The more data structures we treat as immutable, the more code we have in calculations and
  the less we have in actions.

### Immutable data structures are fast enough

* We can always optimize later
* Garbage collectors are really fast
* We're not copying as much as you might think at first
  * shallow copy (just copy the top level of a data structure)
  * structural sharing (copies share a lot of references to the same objects in memory)
* Functional programming languages have fast implementations

### Copy-on-write operations on objects

1. Make a copy.
2. Modify the copy.
3. Return the copy.

```js
// Original
function setPrice(item, new_price) {
    item.price = new_price;
}

// Copy-on-write
function setPrice(item, new_price) {
    var item_copy = Object.assign({}, item);
    item_copy.price = new_price;
    return item_copy;
}
```

Еще:

```js
o["price"] = 37;    // objectSet - copy-on-write version of this

function objectSet(object, key, value) {
    var copy = Object.assign({}, object);
    copy[key] = value;
    return copy;
}

var a = {x : 1};
delete a["x"];    // objectDelete - copy-on-write version of this

function objectDelete(object, key) {
    var copy = Object.assign({}, object);
    delete copy[key];
    return copy;
}
```

### Summary

* In functional programming, we want to use immutable data. It is impossible to write
calculations on mutable data.

* Copy-on-write is a discipline for ensuring our data is immutable. It means we make a
copy and modify it instead of modifying the original.

* Copy-on-write requires making a shallow copy before modifying the copy, then
returning it. It is useful for implementing immutability within code that you control.

* We can implement copy-on-write versions of the basic array and object operations to
reduce the amount of boilerplate we have to write.

## Chapter 7. Staying immutable with untrusted code

Any data that leaves the safe zone is potentially mutable. It could be modified by the
untrusted code. Likewise, any data that enters the safe zone from untrusted code
is potentially mutable.

The copy-on-write pattern won't quite help us here.
We can use *defensive copies* to protect data and maintain immutability.

### Vocab

*Deep copies* duplicate all levels of nested data structures, from the top all the way
to the bottom.

### The rules of defensive copying

Note that these rules could be applied in any order.

#### Rule 1: Copy as data leaves your code

1. Make a deep copy of the immutable data.
2. Pass the copy to the untrusted code.

#### Rule 2: Copy as data enters your code

1. Immediately make a deep copy of the mutable data passed to your code.
2. Use the copy in your code.

### Example

```js
// Original
function add_item_to_cart(name, price) {
    var item = make_cart_item(name, price);
    shopping_cart = add_item(shopping_cart, item);
    var total = calc_total(shopping_cart);
    set_cart_total_dom(total);
    update_shipping_icons(shopping_cart);
    update_tax_dom(total);
    black_friday_promotion(cart_copy);          // untrusted code
}
```

```js
// With defensive copying
function add_item_to_cart(name, price) {
    var item = make_cart_item(name, price);
    shopping_cart = add_item(shopping_cart, item);
    var total = calc_total(shopping_cart);
    set_cart_total_dom(total);
    update_shipping_icons(shopping_cart);
    update_tax_dom(total);
    var cart_copy = deepCopy(shopping_cart);    // Copy before sharing data
    black_friday_promotion(cart_copy);          // untrusted code
    shopping_cart = deepCopy(cart_copy);        // Copy after sharing data
}
```

```js
// Extracted safe version
function add_item_to_cart(name, price) {
    var item = make_cart_item(name, price);
    shopping_cart = add_item(shopping_cart, item);
    var total = calc_total(shopping_cart);
    set_cart_total_dom(total);
    update_shipping_icons(shopping_cart);
    update_tax_dom(total);
    shopping_cart = black_friday_promotion_safe(shopping_cart);     // call extracted code
}

function black_friday_promotion_safe(cart) {     // extracted code
    var cart_copy = deepCopy(cart);
    black_friday_promotion(cart_copy);
    return deepCopy(cart_copy);
}
```

### Copy-on-write and defensive copying compared

#### Copy-on-write

**When to use it**

Use copy-on-write when you need to modify data you control.

**Where to use it**

You should use copy-on-write everywhere inside the safe zone. In fact, the
copy-on-write defines your immutability safe zone.

**Type of copy**

Shallow copy - relatively cheap

**The rules**

1. Make a shallow copy of the thing to modify.
2. Modify the copy.
3. Return the copy.

#### Defensive copying

**When to use it**

Use defensive copying when exchanging data with untrusted code.

**Where to use it**

Use copy-on-write at the borders of your safe zone for data that has to cross in or out.

**Type of copy**

Deep copy - relatively expensive

**The rules**

1. Make a deep copy of data as it enters the safe zone.
2. Make a deep copy of data as it leaves the safe zone.

### Implementing deep copy in JavaScript is difficult

Recommended to using the implementation from the Lodash library (lodash.com).

Пример реализации глубокого копирования.

```js
// (1) - recursively make copies of all of the elements
// (2) - strings, numbers, booleans, and functions are immutable so they don't need to be copied
function deepCopy(thing) {
    if(Array.isArray(thing)) {
        var copy = [];
        for(var i = 0; i < thing.length; i++)
            copy.push(deepCopy(thing[i]));          // (1)
        return copy;
    } else if (thing === null) {
        return null;
    } else if (typeof thing === "object") {
        var copy = {};
        var keys = Object.keys(thing);
        for(var i = 0; i < keys.length; i++) {
            var key = keys[i];
            copy[key] = deepCopy(thing[key]);       // (1)
        }
        return copy;
    } else {
        return thing;       // (2)
    }
}
```

### Summary

*Defensive copying is a discipline for implementing immutability. It makes copies as
data leaves or enters your code.

* Defensive copying makes deep copies, so it is more expensive than copy-on-write.

* Unlike copy-on-write, defensive copying can protect your data from code that does not
implement an immutability discipline.

* We often prefer copy-on-write because it does not require as many copies and we use
defensive copying only when we need to interact with untrusted code.

* Deep copies copy an entire nested structure from top to bottom. Shallow copies only
copy the bare minimum.

## Chapter 8. Stratified design: Part 1

### Vocab

*Stratified design* is a design technique that builds software in layers.
Each layer defines new functions in terms of the functions in the layers below it.

### Пример уровней

1. (Верхний) business rules

  * `gets_free_shipping()`
  * `cartTax()`

2. cart operations

  * `remove_item_by_name()`
  * `calc_total()`
  * `add_item()`
  * `setPriceByName()`

3. copy-on-write

  * `removeItems()`
  * `add_element_last()`

4. (Нижний) array built-ins

  * `.slice()`

### Developing our design sense

#### Характеристики, которые могут использоваться в качестве критериев для дизайна

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

#### Решения, принимаемые при проектировании дизайна

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

1. Выделить desired (нужные) shopping cart operations

* Add an item.
* Remove an item.
* Check if an item is in the cart.
* Sum the total.
* Clear the cart.
* Set the price of an item by name.
* Calculate the tax.
* Check if it gets free shipping.

2. Visualizing our function calls with a call graph

Пример диаграммы:

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

4. Стрелки между функциями из разных слоев должны быть как можно короче.

5. All functions in a layer should serve (выполнять) the same pupose (функцию/работу).

Пример таких слоев:

* business rules about carts (самый верхний)
  * `freeTieClip()`, `gets_free_shipping()`, `cartTax()`
* business rules (general)
  * `calc_tax()`
* basic cart operations
  * `add_item()`, `setPriceByName()`, `isInCart()`, `calc_total()`, `remove_item_by_name()`
* basic item operations
  * `make_item()`, `setPrice()`
* copy-on-write operations
  * `add_element_last()`, `removeItems()`
* JavaScript language features (самый нижний)
  * `object literal`, `.slice()`, `for loop`, `array index`

### Three different zoom levels

1. Global zoom level

  At the global zoom level, we see the entire call graph.

2. Layer zoom level

  At the layer zoom level, we start with the level of interest and draw everything it
  points to below it.

3. Function zoom level

  At the function zoom level, we start with one function of interest and draw everything it
  points to below it.

### Summary

* Stratified design organizes code into layers of abstraction. Each layer helps us ignore
different details.

* When implementing a new function, we need to identify what details are important to solving
the problem. This will tell you what layer the function should be in.

* There are many clues (подсказок) that can help us locate functions in layers. We can look at
the name, the body, and the call graph.

* The name tells us the intent (смысл/цель) of the function. We can group it with other functions
with related (похожими) intents.

* The body can tell us the details that are important to a function. These are clues as to
where in the layer structure it goes.

* The call graph can show us that an implementation is not straightforward. If the arrows
coming from it are of varying lengths, it's a good sign the implementation is not
straightforward.

* We can improve the layer structure by extracting out more general functions. More
general functions are on lower layers and are more reusable.

* The pattern of straightforward implementation guides us to build layers such that our
functions are implemented in a clear and elegant way.

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

#### When to use (and when *not* to use!) abstraction barriers

1. To facilitate (для облегчения) changes of implementation.

  * Abstraction barrier позволяет потом изменять нижележащие слои.

  * This property might be useful if you are prototyping something and you still don't
  know how best to implement it.

  * You know something will change; you're just not ready to do it yet.

2. To make code easier to write and read.

  * An abstraction barrier that lets you ignore lower code details will make your code
  easier to write.

3. To reduce coordination between teams.

  * The abstraction barrier allows teams on either (обеих) side to ignore the details the
  other team handles.

4. To mentally focus on the problem at hand.

#### Не следует делать абстрактный барьер на слишком низком слое иерархии

1. Code in the barrier is lower level, so it's more likely to contain bugs.

2. Low-level code is harder to understand.

### Pattern 3: Minimal interface

By keeping our interfaces minimal, we avoid bloating our lower layers with unnecessary
features.

In stratified design, we find a dynamic tension between the completeness of the abstraction
barrier and the pattern to keep it minimal.

There are many reasons to keep the abstraction barrier minimal:

1. If we add more code to the barrier, we have more to change when we change the
implementation.

2. More functions in an abstraction barrier mean more coordination between teams.

3. A larger interface to our abstraction barrier is harder to keep in your head.

The minimal interface pattern guides us to solve problems at higher levels and avoid
modifying lower levels.

The pattern can be applied to all layers, not just abstraction barriers.

#### Идеальный layer, к которому следует стремиться

1. Layer should have as many functions as necessary, but no more.

2. The functions should not have to change, nor should you need to add functions later.

3. The set should be complete, minimal, and timeless.

### Pattern 4: Comfortable layers

Не надо делать слишком "высокие" башни абстракций. Не надо добавлять слои только ради
спорта. We should invest time in the layers that will help us deliver software
faster and with higher quality. Если нам комфортно работать с текущими уровнями, значит,
скорее всего, они не нуждаются в дополнительных улучшениях.

### What does the graph show us about our code?

*Nonfunctional requirements* (**NFR**s) are things like how testable, maintainable, or
reusable the code is.

Рассматриваются три NFRs:

1. *Maintainability* - What code is easiest to change when requirements change?
2. *Testability* - What is most important to test?
3. *Reusability* - What functions are easier to reuse?

### Code at the top of the graph is easier to change

```text
    A
   / \
  v   v
  B   C
```

`A` - легче изменить, чем `B` или `C`.

The longer the path from the top to a function, the more expensive that function will be to
change.

If we put code that changes frequently near or at the top, our jobs will be easier. Build
less on top of things that change.

### Testing code at the bottom is more important

If we're doing it right, code at the top changes more frequently than code at the bottom.

Тесты функций на верхних слоях иерархии имеют меньший вес/значение, т.к. данный функционал
может меняться очень часто.

Наоборот, тесты функций на нижних слоях иерархии имеют больший вес, т.к. здесь изменения
происходят гораздо реже.

### Code at the bottom is more reusable (более многократно используется)

Чем выше функция в иерархии, тем меньше она пригодна для повторного использования.

### Summary: What the graph shows us about our code

```text
--- A ---       Легче менять. Тесты менее ценны. Меньшая переиспользуемость.
|       |
v       v
B       C       Сложнее менять. Тесты более ценны. Большая переиспользуемость.
```

### Summary

* The pattern of abstraction barrier lets us think at a higher level. Abstraction barriers
let us completely hide details.

* The pattern of minimal interface has us build layers that will converge (стремиться) on
a final form. The interfaces for important business concepts should not grow or change
once they have matured. (Интерфейсы для устоявшегося "взрослого" функционала не должны меняться).

* The pattern of comfort helps us apply the other patterns to serve our needs. It is easy to
over-abstract when applying these patterns. We should apply them with purpose.
(Короче, не следует бесцельно создавать слишком много слоев абстракций).

* Properties emerge (являются следствием) from the structure of the call graph. Those properties
tell us where to put code to maximize our testability, maintainability, and reusability.
