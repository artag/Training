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
