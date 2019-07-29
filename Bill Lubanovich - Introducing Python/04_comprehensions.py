# Включения - спосособ создания структуры данных из одного и более итераторов.

# Включение списка
number_list = [number for number in range(1, 6)]
print(number_list)      # 1 2 3 4 5
print('---')

number_list = [number - 1 for number in range(1, 6)]
print(number_list)      # 0 1 2 3 4
print('---')

odd_numbers = [number for number in range(1, 6) if number % 2 == 1]
print(odd_numbers)     # 1 3 5
print('---')

rows = range(1, 4)
cols = range(1, 3)
cells = [(row, col) for row in rows for col in cols]
for cell in cells:
    print(cell)
# (1, 1)
# (1, 2)
# (2, 1)
# (2, 2)
# (3, 1)
# (3, 2)
print('---')

for row, col in cells:
    print(row, col)
# 1 1
# 1 2
# 2 1
# 2 2
# 3 1
# 3 2

# Включение словаря
word = 'letters'
letter_counts = {letter: word.count(letter) for letter in word}
print(letter_counts)        # ('l': 1, 'e': 2, 't': 2, 'r': 1, 's': 1)
print('---')

# much better
word = 'letters'
letter_count = {letter: word.count(letter) for letter in set(word)}
print(letter_counts)
print('---')

# Включение множества
a_set = {number for number in range(1, 6) if number % 3 == 1}
print(a_set)                    # {1, 4}
print('---')

# Включение генератора
number_thing = (number for number in range(1, 6))
print(type(number_thing))       # <class 'generator'>
print('---')

for number in number_thing:
    print(number)               # 1 2 3 4 5
print('---')

try_again = list(number_thing)
print(try_again)                # []
print('---')
