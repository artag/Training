# Список можно создать из нуля или более элементов, разделенных запятыми и
# заключенных в квадратные скобки:
empty_list = []
weekdays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday']
another_empty_list = list()
print(empty_list)
print(weekdays)
print(another_empty_list)
print('---')

# Функция list() преобразует другие типы данных в списки
print(list('cat'))                      # ['c', 'a', 't']
a_tuple = ('ready', 'fire', 'aim')
print(list(a_tuple))                    # ['ready', 'fire', 'aim']
print('---')

# Можно использовать функцию split(), чтобы преобразовать строку в список,
# указав некую строку-разделитель:
birthday = '1/6/1952'
print(birthday.split('/'))              # ['1', '6', '1952']
splitme = 'a/b//c/d//e'
print(splitme.split('/'))               # ['a', 'b', '', 'c', 'd', '', 'e']
print('---')

# Получение элемента с помощью конструкции [смещение]
marxes = ['Groucho', 'Chico', 'Harpo']
print(marxes[2])                        # Harpo
print(marxes[-2])                       # Chico
print('---')

# Списки списков
small_birds = ['hummingbird', 'finch']
extinct_birds = ['dodo', 'passenger pigeon', 'Norwegian Blue']
carol_birds = [3, 'French hens', 2, 'turtledoves']
all_birds = [small_birds, extinct_birds, 'macaw', carol_birds]
print(all_birds)
print(all_birds[1])         # ['dodo', 'passenger pigeon', 'Norwegian Blue']
print(all_birds[1][2])      # Norwegian Blue
print('---')

# Изменение элемента с помощью конструкции [смещение]
print('Initial list:', marxes)      # ['Groucho', 'Chico', 'Harpo']
marxes[2] = 'Wanda'
print('Changed list:', marxes)      # ['Groucho', 'Chico', 'Wanda']
print('---')
marxes[2] = 'Harpo'

# Извлечение элементов с помощью диапазона смещений
print('marxes[:]', marxes[:])       # ['Groucho', 'Chico', 'Harpo']
print('marxes[0:2]', marxes[0:2])   # ['Groucho', 'Chico']
print('marxes[::2]', marxes[::2])   # ['Groucho', 'Harpo']
print('marxes[::-1]', marxes[::-1]) # ['Harpo', 'Chico', 'Groucho']
print('---')

# Добавление элемента в конец списка с помощью метода append()
print('Initial list:', marxes)
marxes.append('Zeppo')
print('List after append new item:', marxes)
print('---')
marxes.remove('Zeppo')

# Объединяем списки с помощью метода extend() или оператора +=
print('Initial list:', marxes)
others = ['Gummo', 'Karl']
marxes.extend(others)
print('Extended list:', marxes)
marxes = ['Groucho', 'Chico', 'Harpo']
marxes += others
print('Extended list:', marxes)
marxes = ['Groucho', 'Chico', 'Harpo']
marxes.append(others)
print('List after append new items:', marxes)
print('---')

# Добавление элемента с помощью функции insert()
# Удаление заданного элемента с помощью функции del
letters = list('acd')
letters.insert(1, 'b')
print('Insert "b":', letters)           # ['a', 'b', 'c', 'd']
letters.append('e')
print('Append "e":', letters)           # ['a', 'b', 'c', 'd', 'e']
del letters[-1]
print('Delete last symbol:', letters)   # ['a', 'b', 'c', 'd']
print('letters[2] =', letters[2])       # letters[2] = c
del letters[1]
print('Delete letters[1]', letters)     # ['a', 'c', 'd']
print('letters[2] =', letters[2])       # letters[2] = d
print('---')

# Удаление элемента по значению с помощью функции remove()
numbers = ['one', 'two', 'three', 'four', 'five']
print('Initial numbers:', numbers)
numbers.remove('three')
print('List after removed "three":', numbers)       # ['one', 'two', 'four', 'five']
print('---')

# Получение заданного элемента и его удаление с помощью функции pop()
numbers = ['one', 'two', 'three', 'four', 'five']
print('Initial numbers:', numbers)          # ['one', 'two', 'three', 'four', 'five']
removed_item = numbers.pop()
print('After pop() executed:', numbers)     # ['one', 'two', 'three', 'four']
print('Removed_item =', removed_item)       # Removed_item = five
removed_item = numbers.pop(1)
print('After pop(1) executed:', numbers)    # ['one', 'three', 'four']
print('Removed_item =', removed_item)       # Removed_item = two
removed_item = numbers.pop(0)
print('After pop(0) executed:', numbers)    # ['three', 'four']
print('Removed_item =', removed_item)       # Removed_item = one
print('---')

# Определение смещения элемента по значению с помощью функции index()
# Проверка на наличие элемента в списке с помощью оператора in
letters = list('abcd')
print('Letters:', letters)                      # ['a', 'b', 'c', 'd']
print('Index of "c" =', letters.index('c'))     # Index of "c" = 2
print('"d" in letters =', 'd' in letters)       # "d" in letters = True
print('"z" in letters =', 'z' in letters)       # "z" in letters = False
print('---')

# Определяем количество включений значения с помощью функции count()
# Проверка на наличие элемента в списке с помощью оператора in
some_words = ['pizza', 'sushi', 'cola', 'hamburger', 'pizza', 'sushi']
print('Words:', some_words)
print('Pizza count =', some_words.count('pizza'))   # Pizza count = 2
some_words.remove('pizza')
print('After remove "pizza":', some_words)
print('Pizza count =', some_words.count('pizza'))   # Pizza count = 1
print('"Sushi" in list =', 'sushi' in some_words)   # "Sushi" in list = True
del some_words[0]
print('Removed index 0:', some_words)
print('"Sushi" in list =', 'sushi' in some_words)   # "Sushi" in list = True
some_words.pop()
print('Removed last item:', some_words)
print('"Sushi" in list =', 'sushi' in some_words)   # "Sushi" in list = False
print('---')

# Преобразование списка в строку с помощью функции join()
friends = ['Harry', 'Hermione', 'Ron']
print(friends)
separator = ' * '
joined = separator.join(friends)
print(joined)                           # Harry * Hermione * Ron
separated = joined.split(separator)
print(separated)                        # ['Harry', 'Hermione', 'Ron']
print('---')

# Меняем порядок элементов с помощью функции sort()
marxes = ['Groucho', 'Chico', 'Harpo']
sorted_marxes = sorted(marxes)
print('Initial =', marxes)              # Initial = ['Groucho', 'Chico', 'Harpo']
print('Sorted copy =', sorted_marxes)   # Sorted copy = ['Chico', 'Groucho', 'Harpo']
marxes.sort()
print('Initial sorted =', marxes)       # Initial sorted = ['Chico', 'Groucho', 'Harpo']
print('---')

# По умолчанию список сортируется по возрастанию, но вы можете добавить
# аргумент reverse=True , чтобы отсортировать список по убыванию:
numbers = [2, 1, 4.0, 3]
print('Numbers:', numbers)
print('Sorted numbers:', sorted(numbers))                   # [1, 2, 3, 4.0]
print('Sorted numbers:', sorted(numbers, reverse=True))     # [4.0, 3, 2, 1]
print('---')

# Получение длины списка с помощью функции len()
print(len(numbers))
print('---')

# Присваивание с помощью оператора =, копирование с помощью функции copy()
a = [1, 2, 3]
b = a
a[0] = 'oops'
print('a:', a)      # a: ['oops', 2, 3]
print('b:', b)      # b: ['oops', 2, 3]
print('---')

a = [1, 2, 3]
b = a.copy()
c = list(a)
d = a[:]
a[0] = 'ooops!'
print('a:', a)      # a: ['ooops!', 2, 3]
print('b:', b)      # b: [1, 2, 3]
print('c:', c)      # c: [1, 2, 3]
print('d:', d)      # d: [1, 2, 3]
print('---')
