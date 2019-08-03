# Создание множества с помощью функции set()
empty_set = set()
even_numbers = {0, 8, 2, 4, 6}
print(empty_set)
print(even_numbers)
print('---')

# Преобразование других типов данных с помощью функции set()
print(set('letters'))                                               # {'r', 'l', 's', 'e', 't'}
print(set(['Dasher', 'Dancer', 'Prancer', 'Mason-Dixon']))          # {'Mason-Dixon', 'Dancer', 'Prancer', 'Dasher'}
print(set(('Umaguma', 'Echoes', 'Atom Heart Mother')))              # {'Umaguma', 'Atom Heart Mother', 'Echoes'}
print(set({'apple': 'red', 'orange': 'orange', 'cherry': 'red'}))   # {'apple', 'orange', 'cherry'}
print('---')

# Проверяем на наличие значения с помощью ключевого слова in
drinks = {
    'martini': {'vodka', 'vermouth'},
    'black russian': {'vodka', 'kahlua'},
    'white russian': {'cream', 'kahlua', 'vodka'},
    'manhattan': {'rye', 'vermouth', 'bitters'},
    'screwdriver': {'orange juice', 'vodka'}
}
for name, contents in drinks.items():
    if 'vodka' in contents and not ('vermouth' in contents or 'cream' in contents):
        print(name)     # screwdriver, black russian
print('---')

for name, contents in drinks.items():
    if contents & {'vermouth', 'orange juice'}:
        print(name)     # screwdriver, martini, manhattan
print('---')

for name, contents in drinks.items():
    if 'vodka' in contents and not contents & {'vermouth', 'cream'}:
        print(name)     # screwdriver, black russian
print('---')

# Пересечение множеств (члены обоих множеств) можно получить с помощью
# 1. особого пунктуационного символа &
# 2. функции множества intersection()
a = {1, 2}
b = {2, 3}
print('a:', a)
print('b:', b)
print("-- Intersection")
print(a.intersection(b))        # {2}
print('a & b', a & b)           # {2}

# Объединение (члены обоих множеств) можно получить используя
# 1. оператор |
# 2. функцию множества union()
print("-- Union")
print(a.union(b))               # {1, 2, 3}
print('a | b', a | b)           # {1, 2, 3}

# Разность множеств (члены только первого множества, но не второго) можно получить с помощью
# 1. символа –
# 2. функции difference()
print("-- Difference")
print(a.difference(b))          # {1}
print('a - b', a - b)           # {1}

# Исключающее ИЛИ (элементы или первого, или второго множества, но не общие) можно получить используя
# 1. оператор ^
# 2. функцию symmetric_difference()
print("-- Symmetric difference")
print(a.symmetric_difference(b))        # {1, 3}
print('a ^ b', a ^ b)                   # {1, 3}

# Является ли одно множество подмножеством другого
# (все члены первого множества являются членами второго), можно получить с помощью
# 1. оператора <=
# 2. функции issubset()
print("-- Subset")
print(a.issubset(b))        # False
print('a <= b', a <= b)     # False
print('a <= a', a <= a)     # True
print('a < b', a < b)       # False

# Множество множеств противоположно подмножеству
# (все члены второго множества являются также членами первого). Используется
# 1. оператор >=
# 2. функция issuperset()
print("-- Superset")
print(a.issuperset(b))      # False
print('a >= b', a >= b)     # False
print('a >= a', a >= a)     # True
print('a > b', a > b)       # False
