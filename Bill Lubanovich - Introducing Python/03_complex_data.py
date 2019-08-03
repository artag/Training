# Три разных списка:
marxes = ['Groucho', 'Chico', 'Harpo']
pythons = ['Chapman', 'Cleese', 'Gilliam', 'Jones', 'Palin']
stooges = ['Moe', 'Curly', 'Larry']

# Кортеж, который содержит в качестве элементов каждый из этих списков:
tuple_of_lists = marxes, pythons, stooges

# Список, который содержит три списка:
list_of_lists = [marxes, pythons, stooges]

# Словарь из списков:
dict_of_lists = {'Marxes': marxes, 'Pythons': pythons, 'Stooges': stooges}
print('-- Tuple of lists:\n', tuple_of_lists)
print('-- List of lists:\n', list_of_lists)
print('-- Dictionary of lists:\n', dict_of_lists)
print('---')

# Ключи словаря должны быть неизменяемыми,
# поэтому список, словарь или множество не могут быть ключом для другого словаря.
# Но кортеж может быть ключом:
houses = {
    (44.79, -93.14, 285): 'My House',
    (38.89, -77.03, 13): 'The White House'
}

print('Houses:\n', houses)
