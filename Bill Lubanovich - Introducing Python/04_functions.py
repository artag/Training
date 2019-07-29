def do_nothing():
    pass

print(do_nothing())
print('---')


def commentary(color):
    if color == 'red':
        return "It's a tomato."
    elif color == 'green':
        return "It's a green pepper."
    elif color == 'bee purple':
        return "I don't know what it is, but only bees can see it."
    else:
        return "I've never heard of the color " + color + "."

comment = commentary('blue')
print(comment)
print('---')


def menu(wine, entree, dessert):
    return {'wine': wine, 'entree': entree, 'dessert': dessert}

print(menu('chardonnay', 'chicken', 'cake'))
print(menu(entree='beef', dessert='bagel', wine='bordeaux'))
print(menu('frontenac', dessert='flan', entree='fish'))
print('---')


def menu(wine, entree, dessert='pudding'):
    return {'wine': wine, 'entree': entree, 'dessert': dessert}

print(menu('chardonnay', 'chicken'))
print(menu('dunkelfelder', 'duck', 'doughnut'))
print('---')


def buggy(arg, result=[]):
    result.append(arg)
    print(result)

buggy('a')      # ['a']
buggy('b')      # ['a', 'b']
print('---')


def works(arg):
    result = []
    result.append(arg)
    print(result)

works('a')      # ['a']
works('b')      # ['b']
print('---')


def works2(arg, result = None):
    if result is None:
        result = []
    result.append(arg)
    print(result)

works2('a')              #['a']
works2('b')              #['b']
works2('d', ['c'])       #['c', 'd']
print('---')


def print_args(*args):
    print('Positional argument tuple:', args)

# Positional argument tuple: (3, 2, 1, 'wait!', 'uh...')
print_args(3, 2, 1, 'wait!', 'uh...')
print('---')


def print_more(req1, req2, *args):
    print('First', req1)
    print('Second', req2)
    print('Other', args)

# First cap
# Second gloves
# Other ('scarf', 'monocle', 'mustache wax')
print_more('cap', 'gloves', 'scarf', 'monocle', 'mustache wax')
print('---')


def print_kwargs(**kwargs):
    print('Keyword arguments:', kwargs)

# Keyword arguments: {'dessert': 'macaroon', 'wine': 'merlot', 'entree': 'mutton'}
print_kwargs(dessert = 'macaroon', wine = 'merlot', entree = 'mutton')
print('---')


def echo(anything):
    """echo returns its input argument"""
    return anything

# echo returns its input argument
print(echo.__doc__)
print('---')


def run_something(func, arg1, arg2):
    func(arg1, arg2)

def add_args(arg1, arg2):
    print(arg1 + arg2)

run_something(add_args, 4, 2)
print('---')


def run_with_positional_args(func, * args):
    return func(*args)

def sum_args(*args):
    return sum(args)

result = run_with_positional_args(sum_args, 1, 2, 3, 4)
print(result)       # 10
print('---')


def outer(a, b):
    def inner(c, d):
        return c + d
    return inner(a, b)

print(outer(4, 7))      # 11
print('---')


def knights(saying):
    def inner(quote):
        return "We are the knights who say: '%s'" % quote
    return inner(saying)

# We are the knights who say: 'Ni!'
print(knights('Ni!'))
print('---')


# Замыкания
# Замыкание - функция, которая динамически генерируется другой функцией,
#             и обе они могут изменяться и запоминать значения переменных,
#             которые были созданы вне функции.

def knights(saying):
    def inner():
        return "We are the knights who say: '%s'" % saying
    return inner

a = knights('Duck')
b = knights('Hasenpfeffer')

# We are the knights who say: 'Duck'
print(a())
# We are the knights who say: 'Hasenpfeffer'
print(b())
print('---')

