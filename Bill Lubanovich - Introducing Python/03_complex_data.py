marxes = ['Groucho', 'Chico', 'Harpo']
pythons = ['Chapman', 'Cleese', 'Gilliam', 'Jones', 'Palin']
stooges = ['Moe', 'Curly', 'Larry']
tuple_of_lists = marxes, pythons, stooges
list_of_lists = [marxes, pythons, stooges]
dict_of_lists = {'Marxes': marxes, 'Pythons': pythons, 'Stooges': stooges}
print('-- Tuple of lists:\n', tuple_of_lists)
print('-- List of lists:\n', list_of_lists)
print('-- Dictionary of lists:\n', dict_of_lists)
print('---')

houses = {
    (44.79, -93.14, 285): 'My House',
    (38.89, -77.03, 13): 'The White House'
}
print('Houses:\n', houses)
