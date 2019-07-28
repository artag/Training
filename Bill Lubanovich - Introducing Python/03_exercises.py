year_list = [1983, 1984, 1985, 1986, 1987, 1988]
print('Years:', year_list)
third_year = year_list[3]
print('Third year =', third_year)
last_year = year_list[-1]
print('Last year =', last_year)
print('---')

things = ['mozzarella', 'cinderella', 'salmonella']
print('Initial list:', things)
things[1] = things[1].capitalize()
print('Capitalize human:', things)
things[0] = things[0].upper()
print('Cheese in upper case:', things)
del things[-1]
print('After delete the last item:', things)
print('---')

surprise = ['Groucho', 'Chico', 'Harpo']
print('The list:', surprise)
surprise[-1] = surprise[-1].lower()
surprise[-1] = surprise[-1][::-1]
surprise[-1] = surprise[-1].capitalize()
print('Changed the last item:', surprise)
print('---')

words = 'dog', 'cat', 'walrus'
translations = 'chien', 'chat', 'morse'
e2f = dict(zip(words, translations))
print('Result dictionary:\n', e2f)
print('Walrus translation is', e2f['walrus'])
print('---')

f2e = {}
for key, value in e2f.items():
    f2e[value] = key
print('Reverse dictionary:\n', f2e)
print('Chien translation is', f2e['chien'])
print('---')

animals = {
    'cats': ['Henri', 'Grumpy', 'Lucy'],
    'octopi': [],
    'emus': []
}

life = {
    'animals': animals,
    'plants': {},
    'other': {}
}
print('Dictionary "life":\n', life)
print('---')

print(life.keys())
print(life['animals'].keys())
print(life['animals']['cats'])
