year_list = [1983, 1984, 1985, 1986, 1987, 1988]
print('Years:', year_list)
third_year = year_list[3]
print('Third year =', third_year)       # Third year = 1986
last_year = year_list[-1]
print('Last year =', last_year)         # Last year = 1988
print('---')

things = ['mozzarella', 'cinderella', 'salmonella']
print('Initial list:', things)                  # ['mozzarella', 'cinderella', 'salmonella']
things[1] = things[1].capitalize()
print('Capitalize human:', things)              # ['mozzarella', 'Cinderella', 'salmonella']
things[0] = things[0].upper()
print('Cheese in upper case:', things)          # ['MOZZARELLA', 'Cinderella', 'salmonella']
del things[-1]
print('After delete the last item:', things)    # ['MOZZARELLA', 'Cinderella']
print('---')

surprise = ['Groucho', 'Chico', 'Harpo']
print('The list:', surprise)                    # ['Groucho', 'Chico', 'Harpo']
surprise[-1] = surprise[-1].lower()
surprise[-1] = surprise[-1][::-1]
surprise[-1] = surprise[-1].capitalize()
print('Changed the last item:', surprise)       # ['Groucho', 'Chico', 'Oprah']
print('---')

words = 'dog', 'cat', 'walrus'
translations = 'chien', 'chat', 'morse'
e2f = dict(zip(words, translations))            # {'dog': 'chien', 'cat': 'chat', 'walrus': 'morse'}
print('Result dictionary:\n', e2f)
print('Walrus translation is', e2f['walrus'])
print('---')

f2e = {}
for key, value in e2f.items():
    f2e[value] = key
print('Reverse dictionary:\n', f2e)             # {'chien': 'dog', 'chat': 'cat', 'morse': 'walrus'}
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

print(life.keys())                  # dict_keys(['animals', 'plants', 'other'])
print(life['animals'].keys())       # dict_keys(['cats', 'octopi', 'emus'])
print(life['animals']['cats'])      # ['Henri', 'Grumpy', 'Lucy']
