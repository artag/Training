empty_list = []
weekdays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday']
another_empty_list = list()
print(empty_list)
print(weekdays)
print(another_empty_list)
print('---')

print(list('cat'))
a_tuple = ('ready', 'fire', 'aim')
print(list(a_tuple))
print('---')

birthday = '1/6/1952'
print(birthday.split('/'))
splitme = 'a/b//c/d//e'
print(splitme.split('/'))
print('---')

marxes = ['Groucho', 'Chico', 'Harpo']
print(marxes[2])
print(marxes[-2])
print('---')

small_birds = ['hummingbird', 'finch']
extinct_birds = ['dodo', 'passenger pigeon', 'Norwegian Blue']
carol_birds = [3, 'French hens', 2, 'turtledoves']
all_birds = [small_birds, extinct_birds, 'macaw', carol_birds]
print(all_birds)
print(all_birds[1])
print(all_birds[1][2])
print('---')

print('Initial list:', marxes)
marxes[2] = 'Wanda'
print('Changed list:', marxes)
print('---')
marxes[2] = 'Harpo'

print('marxes[:]', marxes[:])
print('marxes[0:2]', marxes[0:2])
print('marxes[::2]', marxes[::2])
print('marxes[::-1]', marxes[::-1])
print('---')

print('Initial list:', marxes)
marxes.append('Zeppo')
print('List after append new item:', marxes)
print('---')
marxes.remove('Zeppo')

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

letters = list('acd')
letters.insert(1, 'b')
print('Insert "b":', letters)
letters.append('e')
print('Append "e":', letters)
del letters[-1]
print('Delete last symbol:', letters)
print('letters[2] =', letters[2])
del letters[1]
print('Delete letters[1]', letters)
print('letters[2] =', letters[2])
print('---')

numbers = ['one', 'two', 'three', 'four', 'five']
print('Initial numbers:', numbers)
numbers.remove('three')
print('List after removed "three":', numbers)
print('---')

numbers = ['one', 'two', 'three', 'four', 'five']
print('Initial numbers:', numbers)
removed_item = numbers.pop()
print('After pop() executed:', numbers)
print('Removed_item =', removed_item)
removed_item = numbers.pop(1)
print('After pop(1) executed:', numbers)
print('Removed_item =', removed_item)
removed_item = numbers.pop(0)
print('After pop(0) executed:', numbers)
print('Removed_item =', removed_item)
print('---')

letters = list('abcd')
print('Letters:', letters)
print('Index of "c" =', letters.index('c'))
print('"d" in letters =', 'd' in letters)
print('"z" in letters =', 'z' in letters)
print('---')

some_words = ['pizza', 'sushi', 'cola', 'hamburger', 'pizza', 'sushi']
print('Words:', some_words)
print('Pizza count =', some_words.count('pizza'))
some_words.remove('pizza')
print('After remove "pizza":', some_words)
print('Pizza count =', some_words.count('pizza'))
print('"Sushi" in list =', 'sushi' in some_words)
del some_words[0]
print('Removed index 0:', some_words)
print('"Sushi" in list =', 'sushi' in some_words)
some_words.pop()
print('Removed last item:', some_words)
print('"Sushi" in list =', 'sushi' in some_words)
print('---')

friends = ['Harry', 'Hermione', 'Ron']
print(friends)
separator = ' * '
joined = separator.join(friends)
print(joined)
separated = joined.split(separator)
print(separated)
print('---')

marxes = ['Groucho', 'Chico', 'Harpo']
sorted_marxes = sorted(marxes)
print('Initial =', marxes)
print('Sorted copy =', sorted_marxes)
marxes.sort()
print('Initial sorted =', marxes)
print('---')

numbers = [2, 1, 4.0, 3]
print('Numbers:', numbers)
print('Sorted numbers:', sorted(numbers))
print('Sorted numbers:', sorted(numbers, reverse=True))
print('---')

print(len(numbers))
print('---')

a = [1, 2, 3]
b = a
a[0] = 'oops'
print('a:', a)
print('b:', b)
print('---')

a = [1, 2, 3]
b = a.copy()
c = list(a)
d = a[:]
a[0] = 'ooops!'
print('a:', a)
print('b:', b)
print('c:', c)
print('d:', d)
print('---')
