# Добавляет пробел между каждым выводимым объектом, а также символ новой стро-
# ки в конце:
print(99, 'bottles', 'would be enough.')
print('---')

# Преобразование типов данных с помощью функции str()
num0 = str(98.6)
num1 = str(1.0e4)
num2 = str(True)
print(num0)         # 98.6
print(num1)         # 10000.0
print(num2)         # True
print('---')

# Пример управляющих символов
print('a\tbc')
print('a\nbc')
print('a\\bc')
print('---')

# Объединяем строки с помощью символа +
print('Release the kraken! ' + 'At once')
print('First word ' 'Second word')
print('---')

a = 'Duck.'
b = a
c = 'Grey Duck!'
print(a + b + c)
print('---')

# Размножаем строки с помощью символа *
start = 'Na ' * 4 + '\n'        # Na Na Na Na
middle = 'Hey ' * 3 + '\n'      # Hey Hey Hey
end = 'Goodbye.'
print(start + start + middle + end)
print('---')

# Извлекаем символ с помощью символов []
letters = 'abcdefghigklmnopqrstuvwxyz'
print(letters[0])               # a
print(letters[1])               # b
print(letters[-1])              # z
print(letters[-2])              # y
print('---')

name = 'Henny'
print(name.replace('H', 'K'))   # Kenny
print('---')

# Извлекаем подстроки с помощью оператора [start : end : step]
print('All letters [:]', letters[:])
print('[20:]', letters[20:])                    # uvwxyz
print('[12:15]', letters[12:15])                # mno
print('[-3:]', letters[-3:])                    # xyz
print('[18:-3]', letters[18:-3])                # stuvw
print('[-6:-2]', letters[-6:-2])                # uvwx
print('[::7]', letters[::7])                    # ahov
print('[4:20:3]', letters[4:20:3])              # ehknqt
print('[19::4]', letters[19::4])                # tx
print('[:21:5]', letters[:21:5])                # afkpu
print('Reverse order [::-1]', letters[::-1])
print('---')

# Получаем длину строки с помощью функции len()
print('Letters length:', len(letters))          # 26
empty = ''
print('Empty length:', len(empty))              # 0
print('---')

# Разделяем строку с помощью функции split()
print('Test split')
todos = 'get gloves,get mask,give cat vitamins,call ambulance'
print(todos.split(','))
print('---')

# Объединяем строки с помощью функции join()
print('Test join')
crypto_list = ['Yeti', 'Bigfoot', 'Loch Ness Monster']
crypto_string = ', '.join(crypto_list)
print(crypto_string)
print('---')

# Развлекаемся со строками
poem = '''All that doth flow we cannot liquid name
Or else would fire and water be the same;
But that is liquid which is moist and wet
Fire that property can never get.
Then 'tis not cold that doth the fire put out
But 'tis the wet that makes it die, no doubt.'''
print('The poem:\n', poem, '\n')

# Сколько символов содержит это стихотворение?
print('Poem length:', len(poem))

# Начинается ли стихотворение с буквосочетания All ?
print('Starts with "All"', poem.startswith('All'))

# Заканчивается ли оно буквосочетанием 'Folks'?
print('End with "folks!"', poem.endswith('Folks'))

# Найдем смещение первого включения слова the :
word = 'the'
print('First index for "the', poem.find(word))

# А теперь — последнего:
print('Last index for "the"', poem.rfind(word))

# Сколько раз встречается трехбуквенное сочетание the ?
print('Count of "the"', poem.count(word))

# Являются ли все символы стихотворения буквами или цифрами?
print('isalnum', poem.isalnum())
print('---')


setup = 'a duck goes into a bar...'
print('Initial string:', setup)

# Удалим символ « . » с обоих концов строки:
print('Strip:', setup.strip('.'))

# Напишем первое слово с большой буквы:
print('Capitalize:', setup.capitalize())

# Напишем все слова с большой буквы:
print('Title:', setup.title())

# Запишем все слова большими буквами:
print('In upper case:', setup.upper())

# Запишем все слова маленькими буквами:
print('In lower case:', setup.lower())

# Сменим регистры букв:
print('Swap case:', setup.swapcase())

# Отцентруем строку в промежутке из 30 пробелов:
print('30 symbols. Center:', setup.center(30))

# Выровняем ее по левому краю:
print('30 symbols. Left:', setup.ljust(30))

# А теперь по правому:
print('30 symbols. Right:', setup.rjust(30))
print('---')

# Заменяем символы с помощью функции replace()
print('Replace: ', setup.replace('duck', 'marmoset'))

# Заменим максимум 100 включений:
print('Replace: ', setup.replace('a ', 'a famous ', 100))
