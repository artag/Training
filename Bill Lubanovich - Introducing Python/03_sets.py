empty_set = set()
even_numbers = {0, 8, 2, 4, 6}
print(empty_set)
print(even_numbers)
print('---')

print(set('letters'))
print(set(['Dasher', 'Dancer', 'Prancer', 'Mason-Dixon']))
print(set(('Umaguma', 'Echoes', 'Atom Heart Mother')))
print(set({'apple': 'red', 'orange': 'orange', 'cherry': 'red'}))
print('---')

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

a = {1, 2}
b = {2, 3}
print('a:', a)
print('b:', b)
print("-- Intersection")
print(a.intersection(b))
print('a & b', a & b)
print("-- Union")
print(a.union(b))
print('a | b', a | b)
print("-- Difference")
print(a.difference(b))
print('a - b', a - b)
print("-- Symmetric difference")
print(a.symmetric_difference(b))
print('a ^ b', a ^ b)
print("-- Subset")
print(a.issubset(b))
print('a <= b', a <= b)
print('a <= a', a <= a)     # True
print('a < b', a < b)
print("-- Superset")
print(a.issuperset(b))
print('a >= b', a >= b)
print('a >= a', a >= a)     # True
print('a > b', a > b)
